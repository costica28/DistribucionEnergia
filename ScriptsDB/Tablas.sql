CREATE DATABASE DistribucionEnergia
GO

USE DistribucionEnergia
GO


CREATE TABLE Tramo(
	idTramo int not null PRIMARY KEY IDENTITY(1,1),
	nombre varchar(10) not null
)
GO

CREATE TABLE Sector(
	idSector int not null PRIMARY KEY IDENTITY(1,1),
	nombre varchar(20) not null
)
GO

CREATE TABLE InformacionEnergia(
	idInformacionEnergia int not null PRIMARY KEY IDENTITY(1,1),
	idTramo int not null FOREIGN KEY(idTramo) REFERENCES Tramo(idTramo),
	idSector int not null FOREIGN KEY(idSector) REFERENCES Sector(idSector),
	costo float not null,
	fecha date not null,
	operacion varchar(10) not null
)
GO