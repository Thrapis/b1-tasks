DROP TABLE balances;
DROP TABLE files;
DROP TABLE accounts;
DROP TABLE classes;
DROP TABLE organisations;
DROP TABLE currencies;

-- -----------------------------

CREATE TABLE currencies(
	id SERIAL PRIMARY KEY,
	code_name CHAR(3) UNIQUE NOT NULL,
	short_name VARCHAR(16) NOT NULL,
	long_name VARCHAR(64) NOT NULL,
	symbol VARCHAR(8) NOT NULL
);

CREATE TABLE organisations(
	id SERIAL PRIMARY KEY,
	name VARCHAR(255) UNIQUE NOT NULL
);

CREATE TABLE classes(
	id SERIAL PRIMARY KEY,
	number CHAR(1) NOT NULL,
	name VARCHAR(255) NOT NULL,
	UNIQUE(number, name)
);

CREATE TABLE accounts(
	id SERIAL PRIMARY KEY,
	class_id INT REFERENCES classes(id),
	currency_id INT REFERENCES currencies(id),
	organisation_id INT REFERENCES organisations(id),
	number CHAR(4) NOT NULL,
	UNIQUE(class_id, currency_id, organisation_id, number)
);

CREATE TABLE files(
	id SERIAL PRIMARY KEY,
	name VARCHAR(255) NOT NULL,
	data_time TIMESTAMP WITHOUT TIME ZONE NOT NULL,
	uploaded TIMESTAMP WITHOUT TIME ZONE NOT NULL
);

CREATE TABLE balances(
	id SERIAL PRIMARY KEY,
	file_id INT REFERENCES files(id),
	account_id INT REFERENCES accounts(id),
	period_start DATE NOT NULL,
	period_end DATE NOT NULL,
	opening_balance_active NUMERIC,
	opening_balance_passive NUMERIC,
	turnover_debit NUMERIC,
	turnover_credit NUMERIC,
	closing_balance_active NUMERIC,
	closing_balance_passive NUMERIC
);

-- ------------------------------------------

INSERT INTO currencies (code_name, short_name, long_name, symbol)
VALUES
('BYN', 'Рубль', 'Белорусский рубль', 'руб.'),
('USD', 'Доллар', 'Доллар США', '$'),
('EUR', 'Евро', 'Евро', '€');
