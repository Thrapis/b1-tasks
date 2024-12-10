CREATE TABLE gen_data(
	some_date DATE,
	some_latin CHAR(10),
	some_rus CHAR(10),
	some_integer INTEGER,
	some_number NUMERIC(10, 8)
);

-- -----------------------------------------------------

CREATE OR REPLACE PROCEDURE import_data(file_path TEXT)
LANGUAGE plpgsql
AS $$
DECLARE
	total_count INT;
    imported_count INT := 0;
    data_file TEXT;
    data_line TEXT;
	columns TEXT[];
BEGIN
    -- Open file
    data_file := pg_read_file(file_path);
	-- Count lines in file
	SELECT COUNT(*) INTO total_count FROM unnest(string_to_array(data_file, E'\r\n'));

    -- Lines iteration
    FOR data_line IN SELECT * FROM unnest(string_to_array(data_file, E'\r\n')) AS t
    LOOP
		-- If empty string - skip
		IF data_line = '' THEN
            CONTINUE;
        END IF;
	
		-- Split in columns
		columns := string_to_array(data_line, '||');
	
        -- Line insertion
        EXECUTE 'INSERT INTO gen_data VALUES ($1::date, $2, $3, $4::integer, $5::numeric)'
		USING columns[1], columns[2], columns[3], columns[4], REPLACE(columns[5], ',', '.');
			
		-- Increment counter
        imported_count := imported_count + 1;

        -- Process logging
        RAISE NOTICE 'Lines imported: % out of %', imported_count, total_count;
    END LOOP;

    RAISE NOTICE 'Completed. Total lines: %', imported_count;
END;
$$;

-- ------------------------------------------------------------

CALL import_data('D:\CodeProjects\B1 Test\B1_Tasks\Task_1\bin\Debug\net8.0\generated\file1.txt');