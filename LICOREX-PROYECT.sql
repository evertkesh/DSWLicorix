\n\nUSE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = N'BD_LICORIX')
BEGIN
    ALTER DATABASE BD_LICORIX SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE BD_LICORIX;
END
GO

CREATE DATABASE BD_LICORIX;
GO

USE BD_LICORIX;
GO\n\nCREATE TABLE Rol
(
    IdRol       INT IDENTITY(1,1) PRIMARY KEY,
    Nombre      VARCHAR(30) NOT NULL UNIQUE,
    Descripcion VARCHAR(100),
    Estado      BIT NOT NULL DEFAULT 1
);
GO\n\nCREATE TABLE Usuario
(
    IdUsuario     INT IDENTITY(1,1) PRIMARY KEY,
    Nombres       VARCHAR(80) NOT NULL,
    Apellidos     VARCHAR(80) NOT NULL,
    Correo        VARCHAR(120) NOT NULL UNIQUE,
    Contrasena    VARCHAR(255) NOT NULL,
    Telefono      VARCHAR(20),
    Direccion     VARCHAR(200),
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    Estado        BIT NOT NULL DEFAULT 1,
    IdRol         INT NOT NULL,

    CONSTRAINT FK_Usuario_Rol FOREIGN KEY (IdRol) REFERENCES Rol(IdRol)
);
GO\n\nCREATE TABLE Categoria
(
    IdCategoria INT IDENTITY(1,1) PRIMARY KEY,
    Nombre      VARCHAR(50) NOT NULL UNIQUE,
    Descripcion VARCHAR(200),
    ImagenURL   VARCHAR(200) NULL,
    Estado      BIT NOT NULL DEFAULT 1
);
GO\n\nCREATE TABLE Marca
(
    IdMarca     INT IDENTITY(1,1) PRIMARY KEY,
    Nombre      VARCHAR(60) NOT NULL UNIQUE,
    PaisOrigen  VARCHAR(50),
    Descripcion VARCHAR(200),
    Estado      BIT NOT NULL DEFAULT 1
);
GO\n\nCREATE TABLE Producto
(
    IdProducto           INT IDENTITY(1,1) PRIMARY KEY,
    Nombre               VARCHAR(120) NOT NULL,
    Descripcion          VARCHAR(MAX),
    Precio               DECIMAL(10,2) NOT NULL,
    Stock                INT NOT NULL,
    ImagenURL            VARCHAR(255),
    GraduacionAlcoholica DECIMAL(4,2),
    VolumenML            INT,
    FechaRegistro        DATETIME NOT NULL DEFAULT GETDATE(),
    Destacado            BIT NOT NULL DEFAULT 0,
    Estado               BIT NOT NULL DEFAULT 1,
    IdCategoria          INT NOT NULL,
    IdMarca              INT NOT NULL,

    CONSTRAINT FK_Producto_Categoria FOREIGN KEY (IdCategoria) REFERENCES Categoria(IdCategoria),
    CONSTRAINT FK_Producto_Marca     FOREIGN KEY (IdMarca)     REFERENCES Marca(IdMarca),
    CONSTRAINT CK_Producto_Precio    CHECK (Precio > 0),
    CONSTRAINT CK_Producto_Stock     CHECK (Stock >= 0),
    CONSTRAINT CK_Producto_Graduacion CHECK (GraduacionAlcoholica >= 0 AND GraduacionAlcoholica <= 100),
    CONSTRAINT CK_Producto_Volumen    CHECK (VolumenML > 0)
);
GO\n\nCREATE TABLE Promocion
(
    IdPromocion   INT IDENTITY(1,1) PRIMARY KEY,
    Nombre        VARCHAR(100) NOT NULL,
    Descripcion   VARCHAR(250),
    TipoDescuento VARCHAR(1) NOT NULL, 
    ValorDescuento DECIMAL(10,2) NOT NULL,
    FechaInicio   DATE NOT NULL,
    FechaFin      DATE NOT NULL,
    Estado        BIT NOT NULL DEFAULT 1,

    CONSTRAINT CK_Promocion_Fechas CHECK (FechaFin >= FechaInicio),
    CONSTRAINT CK_Promocion_Tipo   CHECK (TipoDescuento IN ('p','m')),
    CONSTRAINT CK_Promocion_Valor  CHECK (ValorDescuento > 0)
);
GO\n\nCREATE TABLE ProductoPromocion
(
    IdProductoPromocion INT IDENTITY(1,1) PRIMARY KEY,
    IdProducto          INT NOT NULL,
    IdPromocion         INT NOT NULL,

    CONSTRAINT UQ_ProductoPromocion UNIQUE(IdProducto, IdPromocion),
    CONSTRAINT FK_PP_Producto       FOREIGN KEY (IdProducto)  REFERENCES Producto(IdProducto),
    CONSTRAINT FK_PP_Promocion      FOREIGN KEY (IdPromocion) REFERENCES Promocion(IdPromocion)
);
GO\n\nINSERT INTO Rol (Nombre, Descripcion)
VALUES
('Administrador', 'Acceso total al sistema'),
('Cliente',       'Usuario que realiza compras');
GO\n\nINSERT INTO Usuario (Nombres, Apellidos, Correo, Contrasena, Telefono, Direccion, IdRol)
VALUES
('Aarom',  'Capillo Ramos', 'aarom@licorix.com', '123456', '987654321', 'Lima',       1),
('Carlos', 'Lopez',         'carlos@gmail.com',  '123456', '987111111', 'San Isidro', 2),
('Maria',  'Torres',        'maria@gmail.com',   '123456', '987222222', 'Miraflores', 2),
('Evert',  'Rata Maldonado','evert@gmail.com',   'Evert6240','987000000','Lima',     1);
GO\n\nINSERT INTO Categoria (Nombre, Descripcion, ImagenURL)
VALUES
('Whisky',      'Whiskies nacionales e importados',    '/img/categorias/whisky.png'),
('Ron',         'Rones añejados y premium',           '/img/categorias/ron.png'),
('Vodka',       'Vodkas premium',                     '/img/categorias/vodka.png'),
('Tequila',     'Tequilas reposados y a�ejos',        '/img/categorias/tequila.png'),
('Vino',        'Vinos tintos, blancos y rosados',    '/img/categorias/vino.png'),
('Pisco',       'Piscos premium peruanos',            '/img/categorias/pisco.png'),
('Aguardiente', 'Aguardientes nacionales e importados','/img/categorias/aguardiente.png'),
('Cerveza',     'Cervezas artesanales e importadas',  '/img/categorias/cerveza.png');
GO\n\nINSERT INTO Marca (Nombre, PaisOrigen, Descripcion)
VALUES

('Johnnie Walker',       'Escocia',       'Marca de whisky escoces premium'),
('Jack Daniel''s',      'Estados Unidos','Whisky Tennessee premium'),
('Macallan',             'Escocia',       'Whisky Single Malt de alta gama'),
('Chivas Regal',         'Escocia',       'Whisky escoces premium'),\n\n('Absolut',              'Suecia',        'Vodka premium sueco'),
('Grey Goose',           'Francia',       'Vodka frances premium'),\n\n('Bacardi',              'Puerto Rico',   'Ron internacional'),
('Cartavio',             'Peru',          'Ron peruano premium'),
('Zacapa',               'Guatemala',     'Ron añejado premium'),\n\n('Casillero del Diablo', 'Chile',         'Vinos premium chilenos'),
('Marqu�s de Riscal',    'España',        'Vinos reserva españoles'),
('Trapiche',             'Argentina',     'Vinos argentinos'),\n\n('Tabernero',            'Peru',          'Pisco peruano premium'),
('Santiago Queirolo',    'Peru',          'Pisco y vinos premium'),
('Barsol',               'Peru',          'Pisco premium de exportaci�n'),\n\n('Nectar',               'Colombia',      'Aguardiente colombiano tradicional'),
('Antioqueño',           'Colombia',      'Aguardiente premium colombiano'),\n\n('Corona',               'Mexico',        'Cerveza premium mexicana'),
('Heineken',             'Paises Bajos',  'Cerveza lager premium');
GO\n\nINSERT INTO Producto 
(Nombre, Descripcion, Precio, Stock, ImagenURL, GraduacionAlcoholica, VolumenML, FechaRegistro, Destacado, Estado, IdCategoria, IdMarca)
VALUES

('Johnnie Walker Black Label',          'Whisky escoces de 12 años.',           180.00, 35,  'blacklabel.png',       40.00, 750, GETDATE(), 1, 1, 1, 1),
('Johnnie Walker Blue Label',           'Whisky premium de edicion especial.',  950.00,  8,  'bluelabel.png',        40.00, 750, GETDATE(), 1, 1, 1, 1),
('Jack Daniel''s Old No.7',             'Whisky Tennessee clasico.',            170.00, 25,  'jackdaniels.png',      40.00, 750, GETDATE(), 1, 1, 1, 2),
('Macallan 18 Years',                   'Whisky Single Malt añejado.',         1850.00,  5,  'macallan18.png',       43.00, 750, GETDATE(), 1, 1, 1, 3),
('Chivas Regal 18',                     'Whisky escoces premium.',              420.00, 12,  'chivas18.png',         40.00, 750, GETDATE(), 1, 1, 1, 4),\n\n('Bacardi Reserva Ocho',                'Ron añejado 8 años.',                  185.00, 15,  'bacardi8.png',         40.00, 750, GETDATE(), 0, 1, 2, 7),
('Cartavio XO',                         'Ron premium peruano.',                 120.00, 18,  'cartavioxo.png',       40.00, 750, GETDATE(), 1, 1, 2, 8),
('Ron Zacapa 23',                       'Ron guatemalteco premium.',            320.00, 10,  'zacapa23.png',         40.00, 750, GETDATE(), 1, 1, 2, 9),\n\n('Absolut Original',                    'Vodka sueco premium.',                  95.00, 40,  'absolut.png',          40.00, 750, GETDATE(), 0, 1, 3, 5),
('Grey Goose Original',                 'Vodka frances ultra premium.',         220.00, 20,  'greygoose.png',        40.00, 750, GETDATE(), 1, 1, 3, 6),\n\n('Tequila Reposado',                    'Tequila reposado premium.',            165.00, 12,  'tequilareposado.png',  38.00, 750, GETDATE(), 0, 1, 4, 7),\n\n('Casillero del Diablo Cabernet Sauv.', 'Vino tinto chileno.',                   65.00, 35,  'casillerocabernet.png',13.50, 750, GETDATE(), 0, 1, 5, 10),
('Marques de Riscal Reserva',           'Vino español reserva.',                180.00, 12,  'riscalreserva.png',    14.00, 750, GETDATE(), 1, 1, 5, 11),
('Trapiche Malbec',                     'Vino argentino Malbec.',                72.00, 20,  'trapichemalbec.png',   13.80, 750, GETDATE(), 0, 1, 5, 12),\n\n('Pisco Tabernero Acholado',            'Pisco peruano premium.',                68.00, 28,  'tabernero.png',        42.00, 750, GETDATE(), 0, 1, 6, 13),
('Santiago Queirolo Italia',            'Pisco de uva Italia.',                  82.00, 18,  'queiroloitalia.png',   42.00, 750, GETDATE(), 1, 1, 6, 14),
('Barsol Quebranta',                    'Pisco quebranta premium.',              95.00, 14,  'barsol.png',           42.00, 750, GETDATE(), 1, 1, 6, 15),\n\n('Aguardiente Nectar Club',             'Aguardiente colombiano.',               58.00, 30,  'nectarclub.png',       29.00, 750, GETDATE(), 0, 1, 7, 16),
('Aguardiente Antioqueño Azul',         'Aguardiente premium.',                  62.00, 25,  'antioqueno.png',       29.00, 750, GETDATE(), 0, 1, 7, 17),\n\n('Corona Extra',                        'Cerveza lager mexicana.',               12.00, 120, 'corona.png',            4.50, 355, GETDATE(), 0, 1, 8, 18),
('Heineken Lager',                      'Cerveza lager premium.',                11.50, 150, 'heineken.png',          5.00, 355, GETDATE(), 0, 1, 8, 19);
GO\n\nINSERT INTO Promocion (Nombre, Descripcion, TipoDescuento, ValorDescuento, FechaInicio, FechaFin)
VALUES
('Cyber Days',     'Descuentos especiales durante Cyber Days', 'p', 20.00, '2026-08-01', '2026-08-07'),
('Fiestas Patrias', 'Promocion nacional',                       'p', 15.00, '2026-07-20', '2026-07-31');
GO

INSERT INTO ProductoPromocion (IdProducto, IdPromocion)
VALUES
(1, 1), 
(2, 1), 
(4, 2); 
GO\n\nCREATE PROCEDURE sp_ListarProductos
AS
BEGIN
    SELECT
        P.IdProducto,
        P.Nombre,
        C.Nombre AS Categoria,
        M.Nombre AS Marca,
        P.Precio,
        P.Stock,
        P.GraduacionAlcoholica,
        P.Destacado,
        P.Estado
    FROM Producto P
    INNER JOIN Categoria C ON P.IdCategoria = C.IdCategoria
    INNER JOIN Marca M     ON P.IdMarca     = M.IdMarca
    ORDER BY P.Nombre;
END;
GO

CREATE PROCEDURE sp_ObtenerProductoPorId
(
    @IdProducto INT
)
AS
BEGIN
    SELECT * FROM Producto WHERE IdProducto = @IdProducto;
END;
GO

CREATE PROCEDURE sp_InsertarProducto
(
    @Nombre               VARCHAR(120),
    @Descripcion          VARCHAR(MAX),
    @Precio               DECIMAL(10,2),
    @Stock                INT,
    @ImagenURL            VARCHAR(255),
    @GraduacionAlcoholica DECIMAL(4,2),
    @VolumenML            INT,
    @Destacado            BIT,
    @IdCategoria          INT,
    @IdMarca              INT
)
AS
BEGIN
    INSERT INTO Producto
    (
        Nombre, Descripcion, Precio, Stock, ImagenURL,
        GraduacionAlcoholica, VolumenML, Destacado, IdCategoria, IdMarca
    )
    VALUES
    (
        @Nombre, @Descripcion, @Precio, @Stock, @ImagenURL,
        @GraduacionAlcoholica, @VolumenML, @Destacado, @IdCategoria, @IdMarca
    );
END;
GO

CREATE PROCEDURE sp_ActualizarProducto
(
    @IdProducto           INT,
    @Nombre               VARCHAR(120),
    @Descripcion          VARCHAR(MAX),
    @Precio               DECIMAL(10,2),
    @Stock                INT,
    @ImagenURL            VARCHAR(255),
    @GraduacionAlcoholica DECIMAL(4,2),
    @VolumenML            INT,
    @Destacado            BIT,
    @IdCategoria          INT,
    @IdMarca              INT
)
AS
BEGIN
    UPDATE Producto
    SET
        Nombre               = @Nombre,
        Descripcion          = @Descripcion,
        Precio               = @Precio,
        Stock                = @Stock,
        ImagenURL            = @ImagenURL,
        GraduacionAlcoholica = @GraduacionAlcoholica,
        VolumenML            = @VolumenML,
        Destacado            = @Destacado,
        IdCategoria          = @IdCategoria,
        IdMarca              = @IdMarca
    WHERE IdProducto = @IdProducto;
END;
GO

CREATE PROCEDURE sp_EliminarProducto
(
    @IdProducto INT
)
AS
BEGIN
    UPDATE Producto SET Estado = 0 WHERE IdProducto = @IdProducto;
END;
GO

CREATE PROCEDURE sp_ListarProductosDestacados
AS
BEGIN
    SELECT * FROM Producto WHERE Destacado = 1 AND Estado = 1;
END;
GO

CREATE PROCEDURE sp_ListarNuevosIngresos
AS
BEGIN
    SELECT * FROM Producto WHERE FechaRegistro >= DATEADD(DAY, -7, GETDATE()) AND Estado = 1;
END;
GO

CREATE PROCEDURE sp_ListarProductosEnOferta
AS
BEGIN
    SELECT
        P.IdProducto,
        P.Nombre,
        P.Precio,
        PR.Nombre AS Promocion,
        PR.TipoDescuento,
        PR.ValorDescuento
    FROM Producto P
    INNER JOIN ProductoPromocion PP ON P.IdProducto  = PP.IdProducto
    INNER JOIN Promocion PR         ON PP.IdPromocion = PR.IdPromocion
    WHERE PR.Estado = 1 AND GETDATE() BETWEEN PR.FechaInicio AND PR.FechaFin;
END;
GO

CREATE PROCEDURE sp_BuscarProductos
(
    @Texto VARCHAR(100)
)
AS
BEGIN
    SELECT * FROM Producto WHERE Nombre LIKE '%' + @Texto + '%' AND Estado = 1;
END;
GO

CREATE PROCEDURE sp_ListarProductosPorCategoria
(
    @IdCategoria INT
)
AS
BEGIN
    SELECT * FROM Producto WHERE IdCategoria = @IdCategoria AND Estado = 1;
END;
GO

CREATE PROCEDURE sp_ListarCategorias
AS
BEGIN
    SELECT 
        IdCategoria,
        Nombre,
        Descripcion,
        ImagenURL,
        Estado
    FROM Categoria
    WHERE Estado = 1
    ORDER BY Nombre;
END;
GO\n\nIF OBJECT_ID('dbo.fn_EsAdministrador','FN') IS NOT NULL
    DROP FUNCTION dbo.fn_EsAdministrador;
GO

CREATE FUNCTION dbo.fn_EsAdministrador(@IdUsuario INT)
RETURNS BIT
AS
BEGIN
    DECLARE @res BIT = 0;
    IF EXISTS(
        SELECT 1 FROM Usuario u
        INNER JOIN Rol r ON u.IdRol = r.IdRol
        WHERE u.IdUsuario = @IdUsuario AND u.Estado = 1 AND r.Nombre = 'Administrador'
    ) SET @res = 1;
    RETURN @res;
END;
GO\n\nIF OBJECT_ID('dbo.PrecioHistorial','U') IS NULL
BEGIN
    CREATE TABLE PrecioHistorial
    (
        IdPrecioHist INT IDENTITY(1,1) PRIMARY KEY,
        IdProducto INT NOT NULL,
        PrecioAnterior DECIMAL(10,2) NOT NULL,
        PrecioNuevo DECIMAL(10,2) NOT NULL,
        FechaCambio DATETIME NOT NULL DEFAULT GETDATE(),
        IdUsuario INT NOT NULL,
        CONSTRAINT FK_PrecioHist_Producto FOREIGN KEY (IdProducto) REFERENCES Producto(IdProducto),
        CONSTRAINT FK_PrecioHist_Usuario FOREIGN KEY (IdUsuario) REFERENCES Usuario(IdUsuario)
    );
END;
GO\n\nIF OBJECT_ID('dbo.sp_CambiarPrecioProducto','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_CambiarPrecioProducto;
GO

CREATE PROCEDURE sp_CambiarPrecioProducto
(
    @IdUsuario INT,
    @IdProducto INT,
    @NuevoPrecio DECIMAL(10,2)
)
AS
BEGIN
    SET NOCOUNT ON;

    
    IF dbo.fn_EsAdministrador(@IdUsuario) = 0
    BEGIN
        RAISERROR('Acceso denegado: se requiere privilegios de Administrador.',16,1);
        RETURN;
    END

    
    IF NOT EXISTS(SELECT 1 FROM Producto WHERE IdProducto = @IdProducto AND Estado = 1)
    BEGIN
        RAISERROR('Producto no encontrado o inactivo.',16,1);
        RETURN;
    END

    IF @NuevoPrecio <= 0
    BEGIN
        RAISERROR('El precio debe ser mayor a 0.',16,1);
        RETURN;
    END

    DECLARE @PrecioAnterior DECIMAL(10,2);
    SELECT @PrecioAnterior = Precio FROM Producto WHERE IdProducto = @IdProducto;

    UPDATE Producto SET Precio = @NuevoPrecio WHERE IdProducto = @IdProducto;

    INSERT INTO PrecioHistorial (IdProducto, PrecioAnterior, PrecioNuevo, IdUsuario)
    VALUES (@IdProducto, @PrecioAnterior, @NuevoPrecio, @IdUsuario);
END;
GO\n\nIF OBJECT_ID('dbo.sp_CrearAdministrador','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_CrearAdministrador;
GO

CREATE PROCEDURE sp_CrearAdministrador
(
    @IdUsuarioSolicitante INT,
    @Nombres VARCHAR(80),
    @Apellidos VARCHAR(80),
    @Correo VARCHAR(120),
    @Contrasena VARCHAR(255),
    @Telefono VARCHAR(20) = NULL,
    @Direccion VARCHAR(200) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    IF dbo.fn_EsAdministrador(@IdUsuarioSolicitante) = 0
    BEGIN
        RAISERROR('Acceso denegado: se requiere privilegios de Administrador.',16,1);
        RETURN;
    END

    DECLARE @IdRolAdmin INT = (SELECT IdRol FROM Rol WHERE Nombre = 'Administrador');

    IF EXISTS(SELECT 1 FROM Usuario WHERE Correo = @Correo)
    BEGIN
        
        UPDATE Usuario SET IdRol = @IdRolAdmin, Estado = 1 WHERE Correo = @Correo;
        RETURN;
    END

    INSERT INTO Usuario (Nombres, Apellidos, Correo, Contrasena, Telefono, Direccion, IdRol)
    VALUES (@Nombres, @Apellidos, @Correo, @Contrasena, @Telefono, @Direccion, @IdRolAdmin);
END;
GO\n\nIF OBJECT_ID('dbo.sp_RemoverAdministrador','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_RemoverAdministrador;
GO

CREATE PROCEDURE sp_RemoverAdministrador
(
    @IdUsuarioSolicitante INT,
    @IdUsuarioTarget INT
)
AS
BEGIN
    SET NOCOUNT ON;

    IF dbo.fn_EsAdministrador(@IdUsuarioSolicitante) = 0
    BEGIN
        RAISERROR('Acceso denegado: se requiere privilegios de Administrador.',16,1);
        RETURN;
    END

    
    IF EXISTS(SELECT 1 FROM Usuario WHERE IdUsuario = @IdUsuarioTarget AND Correo = 'evert@gmail.com')
    BEGIN
        RAISERROR('No se puede remover el rol de administrador al usuario principal evert@gmail.com.',16,1);
        RETURN;
    END

    DECLARE @IdRolCliente INT = (SELECT IdRol FROM Rol WHERE Nombre = 'Cliente');
    UPDATE Usuario SET IdRol = @IdRolCliente WHERE IdUsuario = @IdUsuarioTarget;
END;
GO\n\nIF OBJECT_ID('dbo.trg_Usuario_PreventDelete','TR') IS NOT NULL
    DROP TRIGGER dbo.trg_Usuario_PreventDelete;
GO

CREATE TRIGGER dbo.trg_Usuario_PreventDelete
ON dbo.Usuario
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS(SELECT 1 FROM deleted d WHERE d.Correo = 'evert@gmail.com')
    BEGIN
        RAISERROR('No se permite eliminar la cuenta del administrador principal evert@gmail.com.',16,1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    
    DELETE u FROM Usuario u INNER JOIN deleted d ON u.IdUsuario = d.IdUsuario;
END;
GO\n\nIF OBJECT_ID('dbo.trg_Usuario_PreventChangePrincipal','TR') IS NOT NULL
    DROP TRIGGER dbo.trg_Usuario_PreventChangePrincipal;
GO

CREATE TRIGGER dbo.trg_Usuario_PreventChangePrincipal
ON dbo.Usuario
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    
    IF EXISTS(
        SELECT 1
        FROM inserted i
        JOIN deleted d ON i.IdUsuario = d.IdUsuario
        WHERE (i.Correo = 'evert@gmail.com' OR d.Correo = 'evert@gmail.com')
          AND (i.Estado = 0 OR i.IdRol <> d.IdRol)
    )
    BEGIN
        RAISERROR('No se permite desactivar o cambiar el rol del administrador principal evert@gmail.com.',16,1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END;
GO\n\nIF OBJECT_ID('dbo.sp_ListarAdministradoresActivos','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_ListarAdministradoresActivos;
GO

CREATE PROCEDURE sp_ListarAdministradoresActivos
(
    @IdUsuarioSolicitante INT
)
AS
BEGIN
    SET NOCOUNT ON;

    IF dbo.fn_EsAdministrador(@IdUsuarioSolicitante) = 0
    BEGIN
        RAISERROR('Acceso denegado: se requiere privilegios de Administrador.',16,1);
        RETURN;
    END

    DECLARE @IdRolAdmin INT = (SELECT IdRol FROM Rol WHERE Nombre = 'Administrador');

    SELECT IdUsuario, Nombres, Apellidos, Correo, Telefono, Direccion, FechaRegistro
    FROM Usuario
    WHERE IdRol = @IdRolAdmin AND Estado = 1
    ORDER BY Nombres, Apellidos;
END;
GO\n\nIF OBJECT_ID('dbo.sp_VerHistorialPrecios','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_VerHistorialPrecios;
GO

CREATE PROCEDURE sp_VerHistorialPrecios
(
    @IdUsuarioSolicitante INT,
    @IdProducto INT = NULL,
    @FechaDesde DATETIME = NULL,
    @FechaHasta DATETIME = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    IF dbo.fn_EsAdministrador(@IdUsuarioSolicitante) = 0
    BEGIN
        RAISERROR('Acceso denegado: se requiere privilegios de Administrador.',16,1);
        RETURN;
    END

    SELECT
        ph.IdPrecioHist,
        ph.IdProducto,
        p.Nombre AS Producto,
        ph.PrecioAnterior,
        ph.PrecioNuevo,
        ph.FechaCambio,
        u.IdUsuario AS IdUsuarioCambio,
        u.Correo AS UsuarioCorreo,
        u.Nombres + ' ' + u.Apellidos AS UsuarioNombre
    FROM PrecioHistorial ph
    INNER JOIN Producto p ON ph.IdProducto = p.IdProducto
    INNER JOIN Usuario u ON ph.IdUsuario = u.IdUsuario
    WHERE (@IdProducto IS NULL OR ph.IdProducto = @IdProducto)
      AND (@FechaDesde IS NULL OR ph.FechaCambio >= @FechaDesde)
      AND (@FechaHasta IS NULL OR ph.FechaCambio <= @FechaHasta)
    ORDER BY ph.FechaCambio DESC;
END;
GO\n\nIF OBJECT_ID('dbo.sp_InsertarPromocion','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_InsertarPromocion;
GO

CREATE PROCEDURE sp_InsertarPromocion
(
    @Nombre VARCHAR(100),
    @Descripcion VARCHAR(250),
    @TipoDescuento CHAR(1),
    @ValorDescuento DECIMAL(10,2),
    @FechaInicio DATE,
    @FechaFin DATE,
    @Estado BIT
)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Promocion (Nombre, Descripcion, TipoDescuento, ValorDescuento, FechaInicio, FechaFin, Estado)
    OUTPUT INSERTED.IdPromocion
    VALUES (@Nombre, @Descripcion, @TipoDescuento, @ValorDescuento, @FechaInicio, @FechaFin, @Estado);
END;
GO

IF OBJECT_ID('dbo.sp_ActualizarPromocion','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_ActualizarPromocion;
GO

CREATE PROCEDURE sp_ActualizarPromocion
(
    @IdPromocion INT,
    @Nombre VARCHAR(100),
    @Descripcion VARCHAR(250),
    @TipoDescuento CHAR(1),
    @ValorDescuento DECIMAL(10,2),
    @FechaInicio DATE,
    @FechaFin DATE,
    @Estado BIT
)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Promocion
    SET Nombre=@Nombre, Descripcion=@Descripcion, TipoDescuento=@TipoDescuento,
        ValorDescuento=@ValorDescuento, FechaInicio=@FechaInicio, FechaFin=@FechaFin, Estado=@Estado
    WHERE IdPromocion=@IdPromocion;
END;
GO

IF OBJECT_ID('dbo.sp_EliminarPromocion','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_EliminarPromocion;
GO

CREATE PROCEDURE sp_EliminarPromocion
(
    @IdPromocion INT
)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Promocion SET Estado = 0 WHERE IdPromocion = @IdPromocion;
    DELETE FROM ProductoPromocion WHERE IdPromocion = @IdPromocion;
END;
GO

IF OBJECT_ID('dbo.sp_AgregarProductoPromocion','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_AgregarProductoPromocion;
GO

CREATE PROCEDURE sp_AgregarProductoPromocion
(
    @IdProducto INT,
    @IdPromocion INT
)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM ProductoPromocion WHERE IdProducto = @IdProducto AND IdPromocion = @IdPromocion)
        INSERT INTO ProductoPromocion (IdProducto, IdPromocion) VALUES (@IdProducto, @IdPromocion);
END;
GO

IF OBJECT_ID('dbo.sp_RemoverProductoPromocion','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_RemoverProductoPromocion;
GO

CREATE PROCEDURE sp_RemoverProductoPromocion
(
    @IdProducto INT,
    @IdPromocion INT
)
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM ProductoPromocion WHERE IdProducto = @IdProducto AND IdPromocion = @IdPromocion;
END;
GO

IF OBJECT_ID('dbo.sp_ListarProductosPorPromocion','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_ListarProductosPorPromocion;
GO

CREATE PROCEDURE sp_ListarProductosPorPromocion
(
    @IdPromocion INT
)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.IdProducto, p.Nombre, p.Precio, p.Stock, p.ImagenURL
    FROM Producto p
    INNER JOIN ProductoPromocion pp ON p.IdProducto = pp.IdProducto
    WHERE pp.IdPromocion = @IdPromocion;
END;
GO\n\nALTER PROCEDURE sp_ListarProductos AS BEGIN SELECT P.IdProducto, 
P.Nombre, 
C.Nombre AS Categoria, 
M.Nombre AS Marca, 
P.Precio, P.Stock, 
P.GraduacionAlcoholica,
P.ImagenURL, 
P.Destacado, 
P.Estado FROM Producto P INNER JOIN Categoria C ON P.IdCategoria = C.IdCategoria INNER JOIN Marca M ON P.IdMarca = M.IdMarca ORDER BY P.Nombre; END;

UPDATE Producto SET ImagenURL = '/imagenes/productos/johnnie_walker_black_label.jpg' WHERE IdProducto = 1; 
UPDATE Producto SET ImagenURL = '/imagenes/productos/johnnie_walker_blue_label.jpg' WHERE IdProducto = 2; 
UPDATE Producto SET ImagenURL = '/imagenes/productos/jack_daniels_old_n7.jpg' WHERE IdProducto = 3; 
UPDATE Producto SET ImagenURL = '/imagenes/productos/macallan_18_years.jpg' WHERE IdProducto = 4;
UPDATE Producto SET ImagenURL = '/imagenes/productos/absolut.jpg' WHERE IdProducto = 9;
UPDATE Producto SET ImagenURL = '/imagenes/productos/Grey_Goose_Original.jpg' WHERE IdProducto = 10;
UPDATE Producto SET ImagenURL = '/imagenes/productos/Aguardiente_Antioqueño_Azul.jpg' WHERE IdProducto = 19;
UPDATE Producto SET ImagenURL = '/imagenes/productos/Aguardiente_Nectar_Club.jpg' WHERE IdProducto = 18;
UPDATE Producto SET ImagenURL = '/imagenes/productos/BacardiReservaOcho.jpg' WHERE IdProducto = 6;
UPDATE Producto SET ImagenURL = '/imagenes/productos/BarsolQuebranta.jpg' WHERE IdProducto = 17;
UPDATE Producto SET ImagenURL = '/imagenes/productos/CartavioXO.jpg' WHERE IdProducto = 7;
UPDATE Producto SET ImagenURL = '/imagenes/productos/CasillerodelDiabloCabernetSauv..jpg' WHERE IdProducto = 12;
UPDATE Producto SET ImagenURL = '/imagenes/productos/ChivasRegal18.jpg' WHERE IdProducto = 5;
UPDATE Producto SET ImagenURL = '/imagenes/productos/CoronaExtra.jpg' WHERE IdProducto = 20;
UPDATE Producto SET ImagenURL = '/imagenes/productos/HeinekenLager.jpg' WHERE IdProducto = 21;
UPDATE Producto SET ImagenURL = '/imagenes/productos/MarquesdeRiscalReserva.jpg' WHERE IdProducto = 13;
UPDATE Producto SET ImagenURL = '/imagenes/productos/PiscoTaberneroAcholado.jpg' WHERE IdProducto = 15;
UPDATE Producto SET ImagenURL = '/imagenes/productos/RonZacapa23.jpg' WHERE IdProducto = 8;
UPDATE Producto SET ImagenURL = '/imagenes/productos/SantiagoQueiroloItalia.jpeg' WHERE IdProducto = 16;
UPDATE Producto SET ImagenURL = '/imagenes/productos/TequilaReposado.jpg' WHERE IdProducto = 11;
UPDATE Producto SET ImagenURL = '/imagenes/productos/TrapicheMalbec.jpg' WHERE IdProducto = 14;\n\nUPDATE Categoria SET ImagenURL = '/imagenes/productos/whisky.jpg' WHERE IdCategoria = 1; 
UPDATE Categoria SET ImagenURL = '/imagenes/productos/vino.jpg' WHERE IdCategoria = 5; 
UPDATE Categoria SET ImagenURL = '/imagenes/productos/cerveza.jpg' WHERE IdCategoria = 8;\n\nIF OBJECT_ID('dbo.sp_ListarProductosPaginado','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_ListarProductosPaginado;
GO

CREATE PROCEDURE sp_ListarProductosPaginado
(
    @Page INT,
    @PageSize INT,
    @TotalCount INT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT @TotalCount = COUNT(*) FROM Producto WHERE Estado = 1;

    SELECT
        P.IdProducto,
        P.Nombre,
        C.Nombre AS Categoria,
        M.Nombre AS Marca,
        P.Precio,
        P.Stock,
        P.GraduacionAlcoholica,
        P.Destacado,
        P.Estado,
        P.ImagenURL,
        P.IdCategoria,
        P.IdMarca
    FROM Producto P
    INNER JOIN Categoria C ON P.IdCategoria = C.IdCategoria
    INNER JOIN Marca M     ON P.IdMarca     = M.IdMarca
    WHERE P.Estado = 1
    ORDER BY P.Nombre
    OFFSET (@Page-1)*@PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;
GO\n\nIF OBJECT_ID('dbo.sp_DesactivarUsuario','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_DesactivarUsuario;
GO

CREATE PROCEDURE sp_DesactivarUsuario
(
    @IdUsuarioSolicitante INT,
    @IdUsuarioTarget INT
)
AS
BEGIN
    SET NOCOUNT ON;
    IF dbo.fn_EsAdministrador(@IdUsuarioSolicitante) = 0
    BEGIN
        RAISERROR('Acceso denegado: se requiere privilegios de Administrador.',16,1);
        RETURN;
    END
    IF EXISTS(SELECT 1 FROM Usuario WHERE IdUsuario = @IdUsuarioTarget AND Correo = 'evert@gmail.com')
    BEGIN
        RAISERROR('No se permite desactivar la cuenta del administrador principal evert@gmail.com.',16,1);
        RETURN;
    END
    UPDATE Usuario SET Estado = 0 WHERE IdUsuario = @IdUsuarioTarget;
END;
GO\n\nIF OBJECT_ID('dbo.sp_ActivarUsuario','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_ActivarUsuario;
GO

CREATE PROCEDURE sp_ActivarUsuario
(
    @IdUsuarioSolicitante INT,
    @IdUsuarioTarget INT
)
AS
BEGIN
    SET NOCOUNT ON;
    IF dbo.fn_EsAdministrador(@IdUsuarioSolicitante) = 0
    BEGIN
        RAISERROR('Acceso denegado: se requiere privilegios de Administrador.',16,1);
        RETURN;
    END
    UPDATE Usuario SET Estado = 1 WHERE IdUsuario = @IdUsuarioTarget;
END;
GO\n\nIF OBJECT_ID('dbo.sp_ListarUsuariosPaginado','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_ListarUsuariosPaginado;
GO

CREATE PROCEDURE sp_ListarUsuariosPaginado
(
    @Page INT,
    @PageSize INT,
    @TotalCount INT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT @TotalCount = COUNT(*) FROM Usuario WHERE Estado = 1;

    SELECT u.IdUsuario, u.Nombres, u.Apellidos, u.Correo, u.Telefono, u.Direccion, u.FechaRegistro, u.Estado, u.IdRol, r.Nombre AS NombreRol
    FROM Usuario u
    INNER JOIN Rol r ON u.IdRol = r.IdRol
    WHERE u.Estado = 1
    ORDER BY u.Nombres, u.Apellidos
    OFFSET (@Page-1)*@PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;
GO

SELECT IdProducto, Nombre, ImagenURL FROM Producto ORDER BY IdProducto;