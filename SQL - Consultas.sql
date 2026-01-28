/*
    Crear tablas (CREATE TABLE)
*/

-- MySQL
CREATE TABLE 'Tabla' (
	'Columna1' INTEGER NOT NULL AUTO_INCREMENT, 
    'Columna2' VARCHAR(50) NULL, 
	PRIMARY KEY ('Columna1')
)
COLLATE='utf8mb4_spanish_ci' -- Definir COLLATE de la tabla
;

-- SQLite
CREATE TABLE 'Tabla' (
	'Columna1' INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT, 
    'Columna2' VARCHAR(50) NULL
);

-- SQL Server
CREATE TABLE [dbo].[Tabla]
(
	[Columna1] BIGINT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [Columna2] VARCHAR(50) NULL
);

CREATE VIEW 'Vista' AS SELECT * FROM 'Tabla'; -- MySQL/MariaDB | SQLite

CREATE VIEW [dbo].['Vista'] AS SELECT * FROM 'Tabla'; -- SQL Server

-- Definir condición para crear tabla/vista si no existe
CREATE TABLE IF NOT EXISTS 'Tabla' AS -- MySQL/MariaDB | SQLite
-- 
CREATE VIEW IF NOT EXISTS 'Vista' AS -- MySQL/MariaDB | SQLite
-- 

/*
    Llaves/Claves | Asignaciones
*/

-- Definir PRIMARY KEY y autoincremento
'Columna' INTEGER NOT NULL AUTO_INCREMENT, PRIMARY KEY ('Columna') -- MySQL/MariaDB
-- 
'Columna' INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT -- SQLite
-- 
[Columna] BIGINT NOT NULL PRIMARY KEY IDENTITY(1, 1) -- SQL Server | Inicia en 1, y autoincrementa en 1
-- 

-- Solo permitir valores específicos en una columna
'Columna' INTEGER NOT NULL, CHECK ([Columna] IN ('Valor1', 'Valor2')) -- MySQL/MariaDB
-- 
'Columna' INTEGER CHECK([Columna] IN ('Valor1', 'Valor2')) NOT NULL -- SQLite
-- 

-- Crear FOREIGN KEY:
    -- FOREIGN KEY: señala la columna de la Tabla
    -- REFERENCES: señala la tabla externa y la columna de dicha tabla
    -- ON UPDATE CASCADE: habilitar si actualizar al editar registros en tabla externa
    -- ON DELETE CASCADE: habilitar si eliminarar al eliminar registros en tabla externa
FOREIGN KEY ('Columna') REFERENCES 'Tabla'('Columna') ON UPDATE CASCADE ON DELETE CASCADE

/*
    SELECT: manejo de datos que retornan
*/

SELECT * FROM 'Tabla'; -- Mostrar registros

SELECT * FROM 'Tabla' ORDER BY 'Columna' ASC/DESC; -- ORDER BY: ordenar registros de forma ascendente o descendente

SELECT * FROM 'Tabla' ORDER BY 'Columna1' ASC/DESC, 'Columna2' ASC/DESC; -- ORDER BY: ordenar por 2 o más columnas

SELECT COUNT(*) FROM 'Tabla'; -- COUNT(): contar y retornar n° de registros de una tabla

SELECT SUM('Columna') AS 'Columna' FROM 'Tabla'; -- SUM(): retornar suma de todos los valores numéricos de una columna

SELECT MAX('Columna') AS 'Columna' FROM 'Tabla'; -- MAX(): retornar valor numérico más alto de una columna

SELECT MIN('Columna') AS 'Columna' FROM 'Tabla'; -- MIN(): retornar valor numérico más bajo de una columna

SELECT AVG('Columna') AS 'Columna' FROM 'Tabla'; -- AVG(): retornar valor del promedio de una columna

SELECT REPLACE('Columna', ' ', '') AS 'Columna' FROM 'Tabla'; -- REPLACE(): retornar columna con caracteres reemplazados/removidos

SELECT CAST('Columna' AS 'Columna'('Longitud')) AS 'Columna' FROM 'Tabla'; -- CAST(): convertir una columna de un tipo en otro tipo de datos

SELECT 'Columna1', COUNT('Columna2') AS 'Columna' FROM 'Tabla' GROUP BY 'Columna1'; -- Agrupar una columna en base a otra columna

-- CONCAT(): concatenar (unir) columnas
SELECT CONCAT('Columna1', 'Columna2') AS 'Columna' FROM 'Tabla'; -- MySQL/MariaDB | SQL Server
-- 
SELECT 'Columna1' || 'Columna2' AS 'Columna' FROM 'Tabla'; -- SQLite
-- 

-- Agrupar columnas y separarlas con comas en una única columna
SELECT 'Columna1', GROUP_CONCAT('Columna2' SEPARATOR ', ') AS 'Columna' FROM 'Tabla' GROUP BY 'Columna1'; -- MySQL/MariaDB
-- 
SELECT 'Columna1', GROUP_CONCAT('Columna2', ', ') AS 'Columna' FROM 'Tabla' GROUP BY 'Columna1'; -- SQLite
-- 
SELECT 'Columna1', STRING_AGG('Columna2', ', ') AS 'Columna' FROM 'Tabla' GROUP BY 'Columna1'; -- SQL Server
-- 

/*
    Condicionales: IF | SELECT CASE
*/

SELECT IF('Condición' = 'Valor', 'True', 'False') AS 'Columna' FROM 'Tabla' -- IF: MySQL

SELECT IIF('Condición' = 'Valor', 'True', 'False') AS 'Columna' FROM 'Tabla' -- IF: SQLite

-- SELECT CASE
SELECT
    CASE 'Columna'
        WHEN 'Valor1' THEN 'Resultado1'
        WHEN 'Valor2' THEN 'Resultado2'
        WHEN 'Valor3' THEN 'Resultado3'
    END AS 'Columna'
FROM 'Tabla'; -- MySQL/MariaDB | SQLite

/*
    Manejo de datos sin retorno
*/

INSERT INTO 'Tabla' ('Columna1', 'Columna2', 'Columna3') VALUES ('Valor1', 'Valor2', 'Valor3'); -- INSERT INTO: agregar valores nuevos

UPDATE 'Tabla' SET 'Columna1' = 'Valor1', 'Columna2' = 'Valor2', 'Columna3' = 'Valor3'; -- UPDATE: editar valores

DELETE FROM 'Tabla'; -- DELETE: eliminar datos

WHERE ('Columna' = 'Valor'); -- WHERE: Condicional para filtrar datos según el cumplimiento de la misma

/*
    Base de datos: Información
*/

-- Mostrar toda la información sobre las tablas de una base de datos
SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'; -- MySQL/MariaDB | SQL Server
-- 
SELECT * FROM sqlite_master WHERE type = 'table'; -- SQLite
-- 
