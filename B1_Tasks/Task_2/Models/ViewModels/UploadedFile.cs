using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Task_2.Models.ViewModels;

public class UploadedFile
{
    [JsonProperty(PropertyName = "id")]
    public int Id { get; set; }
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }
    [JsonProperty(PropertyName = "dataTime")]
    public DateTime DataTime { get; set; }
    [JsonProperty(PropertyName = "uploaded")]
    public DateTime Uploaded { get; set; }
}
