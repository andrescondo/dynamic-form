USE main;

CREATE TABLE Form (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    FormName NVARCHAR(200),
);

-- Creacion de registros iniciales en tabla Form
INSERT INTO Form (FormName) VALUES ('Personas');
INSERT INTO Form (FormName) VALUES ('Mascotas');

CREATE TABLE Inputs(
    ID INT IDENTITY(1,1) PRIMARY KEY,
    InputsName NVARCHAR(255) NOT NULL,
    InputsType NVARCHAR(255) NOT NULL,
    IDForm INT,
    FOREIGN KEY (IDForm) REFERENCES Form(ID)
)

-- Creación de tabla de personas
CREATE TABLE Personas (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Nombres NVARCHAR(100),
    FechaNacimiento NVARCHAR(100),
    Estatura NVARCHAR(100),
    Apellidos NVARCHAR(100),
    Direccion NVARCHAR(100),
    Correo NVARCHAR(100),
    IDForm INT,
    FOREIGN KEY (IDForm) REFERENCES Form(ID)
);

-- Creacion de inputs relacionados a tabla de personas
INSERT INTO Inputs (InputsName, InputsType, IDForm ) values ('Nombres', 'text', 1);
INSERT INTO Inputs (InputsName, InputsType, IDForm ) values ('FechaNacimiento', 'date', 1);
INSERT INTO Inputs (InputsName, InputsType, IDForm ) values ('Estatura', 'number', 1);
INSERT INTO Inputs (InputsName, InputsType, IDForm ) values ('Apellidos', 'text', 1);
INSERT INTO Inputs (InputsName, InputsType, IDForm ) values ('Direccion', 'text', 1);
INSERT INTO Inputs (InputsName, InputsType, IDForm ) values ('Correo', 'text', 1);


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

INSERT INTO Inputs (InputsName, InputsType, IDForm ) values ('Especie', 'text', 2);
INSERT INTO Inputs (InputsName, InputsType, IDForm ) values ('Raza', 'text', 2);
INSERT INTO Inputs (InputsName, InputsType, IDForm ) values ('Color', 'text', 2);
INSERT INTO Inputs (InputsName, InputsType, IDForm ) values ('Nombre', 'text', 2);