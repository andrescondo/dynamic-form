USE main;

CREATE TABLE Form (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    FormName NVARCHAR(200),
);

-- Creacion de registros iniciales en tabla Form
INSERT INTO Form (FormName) VALUES ('Personas');
INSERT INTO Form (FormName) VALUES ('Mascotas');

-- Creación de tabla de personas
CREATE TABLE Personas (
    IDPersonas INT IDENTITY(1,1) PRIMARY KEY,
    Nombres NVARCHAR(100),
    FechaNacimiento NVARCHAR(100),
    Estatura NVARCHAR(100),
    Apellidos NVARCHAR(100),
    Direccion NVARCHAR(100),
    Correo NVARCHAR(100),
    IDForm INT,
    FOREIGN KEY (IDForm) REFERENCES Form(ID)
);

--Creación de tabla de mascotas
CREATE TABLE Mascotas (
    IDMascota INT IDENTITY(1,1) PRIMARY KEY,
    Especie NVARCHAR(100),
    Raza NVARCHAR(100),
    Color NVARCHAR(100),
    Nombre NVARCHAR(100),
    IDForm INT,
    FOREIGN KEY (IDForm) REFERENCES Form(ID)
);