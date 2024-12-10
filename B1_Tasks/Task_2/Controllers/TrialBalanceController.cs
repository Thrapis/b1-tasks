﻿using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Task_2.Models.Converters;
using Task_2.Models.Database;
using Task_2.Models.Database.Entities;
using Task_2.TrialBalanceExcel;

namespace Task_2.Controllers;

[ApiController]
[Route("[controller]")]
public class TrialBalanceController
{
    private const string _saveDirectory = "./temporary";

    private readonly IDbContextFactory<ApplicationContext> _dbFactory;

    public TrialBalanceController(IDbContextFactory<ApplicationContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    [HttpGet("GetCurrencies")]
    public IEnumerable<Currency> GetCurrencies()
    {
        using var dbContext = _dbFactory.CreateDbContext();

        return dbContext.Currencies.ToList();
    }

    [HttpPost("UploadFile")]
    public async Task UploadFile(IFormFile file)
    {
        // Create directory if doesn't exist
        if (!Directory.Exists(_saveDirectory))
        {
            Directory.CreateDirectory(_saveDirectory);
        }

        // Saving file
        var fileFullPath = Path.Combine(_saveDirectory, file.FileName);
        using (var fs = new FileStream(fileFullPath, FileMode.Create))
        {
            await file.CopyToAsync(fs);
        }

        // Getting data from file
        var fileData = DataExtraction.GetDataFromFile(fileFullPath);

        // Create context using factory
        using var dbContext = _dbFactory.CreateDbContext();
        
        // Getting base entities from file data
        var fileMeta = ExcelDataToEntity.GetFileFromData(fileData);
        var fileCurrecy = ExcelDataToEntity.GetCurrencyFromData(fileData);
        var fileOrganisation = ExcelDataToEntity.GetOrganisationFromData(fileData);

        using (var transaction = dbContext.Database.BeginTransaction())
        {
            // Handling file currency
            Currency? actualCurrency = await dbContext.Currencies
                .Where(c => c.Symbol == fileCurrecy.Symbol).SingleOrDefaultAsync();
            if (actualCurrency is null)
            {
                throw new Exception("Unknown currency in file");
            }

            // Handling file organisation
            Organisation? actualOrganisation = null;
            if (await dbContext.Organisations.AnyAsync(o => o.Name == fileOrganisation.Name))
            {
                actualOrganisation = await dbContext.Organisations
                    .Where(o => o.Name == fileOrganisation.Name).SingleAsync();
            }
            else
            {
                var result = await dbContext.Organisations.AddAsync(fileOrganisation);
                actualOrganisation = result.Entity;
                await dbContext.SaveChangesAsync();
            }

            // Inserting file record
            var fileAddResult = await dbContext.Files.AddAsync(fileMeta);
            var actualFile = fileAddResult.Entity;
            await dbContext.SaveChangesAsync();

            // Getting class, account and balance data
            var fileClasses = ExcelDataToEntity.GetClassAndNestedFromData(fileData);

            // Handling account classes
            foreach (var fileClass in fileClasses)
            {
                // Check for existing class
                AccountClass? actualClass = null;
                if (await dbContext.AccountClasses.AnyAsync(ac => ac.Name == fileClass.Name && ac.Number == fileClass.Number))
                {
                    actualClass = await dbContext.AccountClasses
                        .Where(ac => ac.Name == fileClass.Name && ac.Number == fileClass.Number).SingleAsync();
                }
                else
                {
                    var result = await dbContext.AccountClasses.AddAsync(fileClass);
                    actualClass = result.Entity;
                    await dbContext.SaveChangesAsync();
                }

                // Handling accounts
                foreach (var fileAccount in fileClass.Accounts)
                {
                    // Check for existing account
                    Account? actualAccount = null;
                    if (await dbContext.Accounts.AnyAsync(a => a.Number == fileAccount.Number &&
                        a.ClassId == actualClass.Id && a.CurrencyId == actualCurrency.Id &&
                        a.OrganisationId == actualOrganisation.Id))
                    {
                        actualAccount = await dbContext.Accounts
                            .Where(a => a.Number == fileAccount.Number &&
                                a.ClassId == actualClass.Id && a.CurrencyId == actualCurrency.Id &&
                                a.OrganisationId == actualOrganisation.Id).SingleAsync();
                    }
                    else
                    {
                        fileAccount.ClassId = actualClass.Id;
                        fileAccount.CurrencyId = actualCurrency.Id;
                        fileAccount.OrganisationId = actualOrganisation.Id;
                        var result = await dbContext.Accounts.AddAsync(fileAccount);
                        actualAccount = result.Entity;
                        await dbContext.SaveChangesAsync();
                    }

                    // Getting balance record
                    var fileBalance = fileAccount.Balances.FirstOrDefault();
                    if (fileBalance is null)
                    {
                        continue;
                    }

                    // Inserting balance record
                    fileBalance.AccountId = actualAccount.Id;
                    fileBalance.FileId = actualFile.Id;
                    await dbContext.Balances.AddAsync(fileBalance);
                }
            }
            await dbContext.SaveChangesAsync();

            // Save changes
            await transaction.CommitAsync();
        }
            

        // Algorithm could be much better if we used stored procedures, but I don't have time for that :(
    }
}