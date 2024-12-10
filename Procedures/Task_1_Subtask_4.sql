CREATE OR REPLACE PROCEDURE calc_statistics()
LANGUAGE plpgsql
as $$
DECLARE
	integer_sum BIGINT;
	number_median NUMERIC(10, 8);
BEGIN
	-- Select sum of integers
	SELECT SUM(t.some_integer) INTO integer_sum FROM gen_data as t;
	-- Select median of fractional numbers (50th percentile)
	SELECT PERCENTILE_CONT(0.5) INTO number_median WITHIN GROUP(ORDER BY t.some_number) FROM gen_data as t;
	
	RAISE NOTICE 'Sum of integers: %', integer_sum;
	RAISE NOTICE 'Median of fractional numbers: %', number_median;	
END;
$$

-- ---------------------------------------------

CALL calc_statistics();

-- ---------------------------------------------
-- Alternative way

SELECT
	SUM(t.some_integer) Sum_Of_Integers,
	(PERCENTILE_CONT(0.5) WITHIN GROUP(ORDER BY t.some_number)) Median_Of_Fractional_Numbers
FROM gen_data as t;