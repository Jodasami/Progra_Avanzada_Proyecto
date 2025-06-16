----************************************ CREACION BD ************************************
--CREATE DATABASE VentaMusical;
--GO

--USE VentaMusical;
--GO

--CREATE TABLE Usuarios(
--	ID						INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
--	NumeroIdentificacion	VARCHAR (30) UNIQUE NOT NULL,
--	Nombre					VARCHAR(30) NOT  NULL,
--	Apellido				VARCHAR(50) NOT  NULL,
--	Genero					NVARCHAR(10)  NOT NULL,
--	CorreoElectronico		VARCHAR(255) NOT NULL,
--	TipoTarjeta				NVARCHAR(25)  NOT NULL,
--	DineroDisponible		DECIMAL(10,2) NOT NULL,
--	NumeroTarjeta			VARCHAR(25) NOT NULL,
--	Contrasena				VARCHAR(30) NOT NULL,
--	Perfil					NVARCHAR(25)  NOT NULL
--);
--GO

--CREATE TABLE Generos(
--	CodigoGenero		INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
--	Descripcion			VARCHAR(100) NOT NULL
--);
--GO

--CREATE TABLE ARTISTAS(
--	CodigoArtista		INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
--	NombreArtistico		VARCHAR(100)NOT NULL,
--	FechaNacimiento		VARCHAR(100)NOT NULL,
--	NombreReal			VARCHAR(100)NOT NULL,
--	Nacionalidad		VARCHAR(100)NOT NULL,
--	Foto				VARCHAR(100)NOT NULL,
--	LinkBiografia		VARCHAR(MAX)NOT NULL,
--);
--GO

--CREATE TABLE Albumes(
--	CodigoAlbum		INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
--	CodigoArtista	INT NOT NULL,
--	NombreAlbum		VARCHAR(150)NOT NULL,
--	AnoLanzamiento	INT NOT NULL,
--	Imagen			VARCHAR(150)NOT NULL,
--	FOREIGN KEY (CodigoArtista) REFERENCES Artistas(CodigoArtista) ON DELETE CASCADE
--);
--GO

--CREATE TABLE Canciones(
--	CodigoCancion		 INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
--	CodigoGenero		 INT NOT NULL,
--	CodigoAlbum			 INT NOT NULL,
--	NombreCancion		 VARCHAR(150) NOT NULL,
--	LinkVideo			 VARCHAR(MAX) NOT NULL,
--	Precio				 DECIMAL(10,2)	NOT NULL,
--	CantidadDisponible	 INT NOT NULL,
--	FOREIGN KEY (CodigoGenero) REFERENCES Generos(CodigoGenero),
--	FOREIGN KEY (CodigoAlbum) REFERENCES Albumes(CodigoAlbum) ON DELETE CASCADE
--);
--GO

--CREATE TABLE Ventas(
--    NumeroFactura     INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
--    IDUsuario         INT NOT NULL,
--    FechaCompra       DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
--    Total             DECIMAL(10,2) NOT NULL,
--    TipoPago          NVARCHAR(25)  NOT NULL,
--    FOREIGN KEY (IDUsuario) REFERENCES Usuarios(ID) ON DELETE NO ACTION
--);
--GO

--CREATE TABLE DetalleVenta(
--	IDDetalle		INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
--	NumeroFactura	INT NOT NULL,
--	CodigoCancion	INT NOT NULL,
--	Subtotal		DECIMAL(10,2) NOT NULL,
--	FOREIGN KEY (NumeroFactura) REFERENCES Ventas(NumeroFactura),
--	FOREIGN KEY (CodigoCancion) REFERENCES Canciones(CodigoCancion) 
--);
--GO

--CREATE TABLE Auditoria(
--	ID					INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
--	FechaRegistro		DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
--	Usuario				VARCHAR(100)	NOT NULL,
--	Accion				NVARCHAR (30)	NOT NULL,
--	TablaAfectada		VARCHAR(100) NOT NULL,
--	IDRegistroAfectado	INT NOT NULL,
--	Detalles			VARCHAR(100) NOT NULL,
--	IPUsuario			VARCHAR(35) NOT NULL
--);
--GO

---- CREACI�N DE INDEX ------------;

---- --�ndices para la tabla Usuarios
--CREATE INDEX IX_Usuarios_NumeroIdentificacion ON Usuarios(NumeroIdentificacion);
--CREATE INDEX IX_Usuarios_CorreoElectronico ON Usuarios(CorreoElectronico);

---- --�ndices para la tabla Generos
--CREATE INDEX IX_Generos_Descripcion ON Generos(Descripcion);

---- --�ndices para la tabla Artistas
--CREATE INDEX IX_Artistas_NombreArtistico ON Artistas(NombreArtistico);

---- --�ndices para la tabla Albumes
--CREATE INDEX IX_Albumes_CodigoArtista ON Albumes(CodigoArtista);
--CREATE INDEX IX_Albumes_NombreAlbum ON Albumes(NombreAlbum);

---- --�ndices para la tabla Canciones
--CREATE INDEX IX_Canciones_CodigoGenero ON Canciones(CodigoGenero);
--CREATE INDEX IX_Canciones_CodigoAlbum ON Canciones(CodigoAlbum);
--CREATE INDEX IX_Canciones_NombreCancion ON Canciones(NombreCancion);

---- --�ndices para la tabla Ventas
--CREATE INDEX IX_Ventas_IDUsuario ON Ventas(IDUsuario);
--CREATE INDEX IX_Ventas_FechaCompra ON Ventas(FechaCompra);

---- --�ndices para la tabla DetalleVenta
--CREATE INDEX IX_DetalleVenta_NumeroFactura ON DetalleVenta(NumeroFactura);
--CREATE INDEX IX_DetalleVenta_CodigoCancion ON DetalleVenta(CodigoCancion);

---- --�ndices para la tabla Auditoria
--CREATE INDEX IX_Auditoria_Usuario ON Auditoria(Usuario);
--CREATE INDEX IX_Auditoria_FechaRegistro ON Auditoria(FechaRegistro);
--CREATE INDEX IX_Auditoria_TablaAfectada ON Auditoria(TablaAfectada);


----************************************ FIN CREACION BD ************************************



----****************************** PROCEDIMIENTOS ALMACENADOS ******************************

---- Albumes (Freddy) ----
-- ________________________Insertar Album________________________----
--CREATE PROCEDURE spInsertarAlbum
--    @CodigoArtista  INT,
--    @NombreAlbum    VARCHAR(150),
--    @AnoLanzamiento INT,
--    @Imagen         VARCHAR(150)
--AS
--BEGIN
    
--    IF NOT EXISTS (SELECT 1 FROM Artistas WHERE CodigoArtista = @CodigoArtista)
--    BEGIN
        
--        RAISERROR('Error: El artista con el código especificado no existe.', 16, 1);
--        RETURN;
--    END

--    BEGIN TRY
--        BEGIN TRANSACTION;

--        INSERT INTO Albumes (CodigoArtista, NombreAlbum, AnoLanzamiento, Imagen)
--        VALUES (@CodigoArtista, @NombreAlbum, @AnoLanzamiento, @Imagen);
        
--        COMMIT;
--    END TRY
--    BEGIN CATCH
--        ROLLBACK;
--        THROW;
--    END CATCH
--END
--GO

-- ________________________Actualizar Album________________________----
--CREATE PROCEDURE spActualizarAlbum
--    @CodigoAlbum    INT,
--    @CodigoArtista  INT,
--    @NombreAlbum    VARCHAR(150),
--    @AnoLanzamiento INT,
--    @Imagen         VARCHAR(150)
--AS
--BEGIN
--    IF NOT EXISTS (SELECT 1 FROM Artistas WHERE CodigoArtista = @CodigoArtista)
--    BEGIN
--        RAISERROR('Error: El artista con el código especificado no existe.', 16, 1);
--        RETURN;
--    END

--    BEGIN TRY
--        BEGIN TRANSACTION;

--        UPDATE Albumes
--        SET
--            CodigoArtista = @CodigoArtista,
--            NombreAlbum = @NombreAlbum,
--            AnoLanzamiento = @AnoLanzamiento,
--            Imagen = @Imagen
--        WHERE
--            CodigoAlbum = @CodigoAlbum;

--        COMMIT;
--    END TRY
--    BEGIN CATCH
--        ROLLBACK;
--        THROW;
--    END CATCH
--END
--GO

-- ________________________Eliminar Album________________________----
--CREATE PROCEDURE spEliminarAlbum
--    @CodigoAlbum INT
--AS
--BEGIN
    
--   DELETE FROM Albumes
--    WHERE CodigoAlbum = @CodigoAlbum;
--END
--GO

-- ________________________Listar todos los Albumes________________________----
--CREATE PROCEDURE spListarAlbumes
--AS
--BEGIN
--    SELECT CodigoAlbum, CodigoArtista, NombreAlbum, AnoLanzamiento, Imagen
--    FROM Albumes
--    ORDER BY NombreAlbum;
--END
--GO

-- ________________________Listar Album por ID________________________----
--CREATE PROCEDURE spListarAlbumPorID
--    @CodigoAlbum INT
--AS
--BEGIN
--    SELECT CodigoAlbum, CodigoArtista, NombreAlbum, AnoLanzamiento, Imagen
--    FROM Albumes
--    WHERE CodigoAlbum = @CodigoAlbum;
--END
--GO

---- Artistas (Freddy) ----
-- ________________________Agregar Artista________________________ ----
--CREATE PROCEDURE spAgregarArtista
--    @NombreArtistico    VARCHAR(100),
--    @FechaNacimiento    VARCHAR(100),
--    @NombreReal         VARCHAR(100),
--    @Nacionalidad       VARCHAR(100),
--    @Foto               VARCHAR(100),
--    @LinkBiografia      VARCHAR(MAX)
--AS
--BEGIN
--    BEGIN TRY
--        BEGIN TRANSACTION;

--        INSERT INTO ARTISTAS (NombreArtistico, FechaNacimiento, NombreReal, Nacionalidad, Foto, LinkBiografia)
--        VALUES (@NombreArtistico, @FechaNacimiento, @NombreReal, @Nacionalidad, @Foto, @LinkBiografia);

--        COMMIT;
--    END TRY
--    BEGIN CATCH
--        ROLLBACK;
--        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
--        RAISERROR('Error al agregar al artista: %s', 16, 1, @ErrorMessage);
--    END CATCH
--END;
--GO

-- ________________________Actualizar Artista________________________ ----
--CREATE PROCEDURE spActualizarArtista
--    @CodigoArtista      INT,
--    @NombreArtistico    VARCHAR(100),
--    @FechaNacimiento    VARCHAR(100),
--    @NombreReal         VARCHAR(100),
--    @Nacionalidad       VARCHAR(100),
--    @Foto               VARCHAR(100),
--    @LinkBiografia      VARCHAR(MAX)
--AS
--BEGIN
--    BEGIN TRY
--        BEGIN TRANSACTION;

--        UPDATE ARTISTAS
--        SET 
--            NombreArtistico = @NombreArtistico,
--            FechaNacimiento = @FechaNacimiento,
--            NombreReal = @NombreReal,
--            Nacionalidad = @Nacionalidad,
--            Foto = @Foto,
--            LinkBiografia = @LinkBiografia
--        WHERE 
--            CodigoArtista = @CodigoArtista;

--        COMMIT;
--    END TRY
--    BEGIN CATCH
--        ROLLBACK;
--        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
--        RAISERROR('Error al actualizar el artista: %s', 16, 1, @ErrorMessage);
--    END CATCH
--END;
--GO

-- ________________________Eliminar Artista________________________ ----
--CREATE PROCEDURE spEliminarArtista
--    @CodigoArtista INT
--AS
--BEGIN
--    DELETE
--    FROM Artistas
--    WHERE CodigoArtista = @CodigoArtista;
--END;
--GO

-- ________________________Mostrar Artistas________________________ ----
--CREATE PROCEDURE spMostrarTodosArtistas
--AS
--BEGIN
--    SELECT CodigoArtista, NombreArtistico, FechaNacimiento, NombreReal, Nacionalidad, Foto, LinkBiografia
--    FROM Artistas;
--END;
--GO

-- ________________________Mostrar Artista Por Id________________________ ----
--CREATE PROCEDURE spMostrarArtistaPorId
--    @CodigoArtista INT
--AS
--BEGIN
--    SELECT CodigoArtista, NombreArtistico, FechaNacimiento, NombreReal, Nacionalidad, Foto, LinkBiografia
--    FROM Artistas
--    WHERE CodigoArtista = @CodigoArtista;
--END;
--GO



---- Auditoria (Fabio)


----                         ***** PROCEDIMIENTOS ALMACENADOS FABIO. INICIO*****


----											Canciones

					----________________________Insertar________________________----

--CREATE PROCEDURE spAgregarCancion
--    @CodigoGenero         INT,
--    @CodigoAlbum          INT,
--    @NombreCancion        VARCHAR(150),
--    @LinkVideo            VARCHAR(MAX),
--    @Precio               DECIMAL(10,2),
--    @CantidadDisponible   INT
--AS
--BEGIN
--    BEGIN TRY
--        BEGIN TRANSACTION;

--        INSERT INTO Canciones 
--        (CodigoGenero, CodigoAlbum, NombreCancion, LinkVideo, Precio, CantidadDisponible)
--        VALUES 
--        (@CodigoGenero, @CodigoAlbum, @NombreCancion, @LinkVideo, @Precio, @CantidadDisponible);

--        COMMIT;
--    END TRY
--    BEGIN CATCH
--        ROLLBACK;
--        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
--        SELECT 
--            @ErrorMessage = ERROR_MESSAGE(),
--            @ErrorSeverity = ERROR_SEVERITY(),
--            @ErrorState = ERROR_STATE();
--        RAISERROR('Error al agregar la canción: %s', @ErrorSeverity, @ErrorState, @ErrorMessage);
--    END CATCH
--END

----________________________Actualizar________________________----

--CREATE PROCEDURE spActualizarCancion
--    @CodigoCancion        INT,
--    @CodigoGenero         INT,
--    @CodigoAlbum          INT,
--    @NombreCancion        VARCHAR(150),
--    @LinkVideo            VARCHAR(MAX),
--    @Precio               DECIMAL(10,2),
--    @CantidadDisponible   INT
--AS
--BEGIN
--    BEGIN TRY
--        BEGIN TRANSACTION;

--        UPDATE Canciones
--        SET
--            CodigoGenero = @CodigoGenero,
--            CodigoAlbum = @CodigoAlbum,
--            NombreCancion = @NombreCancion,
--            LinkVideo = @LinkVideo,
--            Precio = @Precio,
--            CantidadDisponible = @CantidadDisponible
--        WHERE CodigoCancion = @CodigoCancion;

--        COMMIT;
--    END TRY
--    BEGIN CATCH
--        ROLLBACK;
--        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
--        SELECT 
--            @ErrorMessage = ERROR_MESSAGE(),
--            @ErrorSeverity = ERROR_SEVERITY(),
--            @ErrorState = ERROR_STATE();
--        RAISERROR('Error al actualizar la canción: %s', @ErrorSeverity, @ErrorState, @ErrorMessage);
--    END CATCH
--END


----________________________Eliminar________________________----

--CREATE PROCEDURE spEliminarCancion
--    @CodigoCancion INT
--AS
--BEGIN
--    DELETE FROM Canciones
--    WHERE CodigoCancion = @CodigoCancion;
--END


----________________________Buscar Todas________________________----

--CREATE PROCEDURE spMostrarTodasCanciones
--AS
--BEGIN
--    SELECT CodigoCancion, CodigoGenero, CodigoAlbum, NombreCancion, LinkVideo, Precio, CantidadDisponible
--    FROM Canciones;
--END


----________________________Buscar Por ID________________________----

--CREATE PROCEDURE spMostrarCancionPorCodigo
--    @CodigoCancion INT
--AS
--BEGIN
--    SELECT CodigoCancion, CodigoGenero, CodigoAlbum, NombreCancion, LinkVideo, Precio, CantidadDisponible
--    FROM Canciones
--    WHERE CodigoCancion = @CodigoCancion;
--END


----											Auditoria 

----________________________Mostrar Todos________________________----

--CREATE PROCEDURE spMostrarAuditoria
--AS
--BEGIN
--    SELECT 
--        ID,
--        FechaRegistro,
--        Usuario,
--        Accion,
--        TablaAfectada,
--        IDRegistroAfectado,
--        Detalles,
--        IPUsuario
--    FROM Auditoria
--    ORDER BY FechaRegistro DESC;
--END


----________________________Mostrar Tabla Afectada________________________----

--CREATE PROCEDURE spMostrarAuditoriaPorTabla
--    @TablaAfectada VARCHAR(100)
--AS
--BEGIN
--    SELECT 
--        ID,
--        FechaRegistro,
--        Usuario,
--        Accion,
--        TablaAfectada,
--        IDRegistroAfectado,
--        Detalles,
--        IPUsuario
--    FROM Auditoria
--    WHERE TablaAfectada = @TablaAfectada
--    ORDER BY FechaRegistro DESC;
--END


----________________________Mostrar Por Rango de Fecha________________________----

--CREATE PROCEDURE spMostrarAuditoriaPorFecha
--    @FechaInicio DATETIME,
--    @FechaFin    DATETIME
--AS
--BEGIN
--    SELECT 
--        ID,
--        FechaRegistro,
--        Usuario,
--        Accion,
--        TablaAfectada,
--        IDRegistroAfectado,
--        Detalles,
--        IPUsuario
--    FROM Auditoria
--    WHERE FechaRegistro BETWEEN @FechaInicio AND @FechaFin
--    ORDER BY FechaRegistro DESC;
--END


----________________________Dato Importante________________________----

--Como es una tabla de auditoria no se debe permitir las acciones de insertar datos, 
--modificarlos o eliminarlos. Solamente se permite consultarlos.

----						 ***** PROCEDIMIENTOS ALMACENADOS FABIO. FIN*****

---- Detalles (Raquel)
----________________________Insertar________________________----

--CREATE PROCEDURE InsertarDetalleVenta    
--	@NumeroFactura  INT, 
--	@CodigoCancion   INT,
--	@Subtotal		DECIMAl(10,2)

--AS
--BEGIN
--    IF EXISTS (SELECT 1 FROM Ventas WHERE NumeroFactura = @NumeroFactura)
--       AND EXISTS (SELECT 1 FROM Canciones WHERE CodigoCancion = @CodigoCancion)
--    BEGIN
--        INSERT INTO DetalleVenta (NumeroFactura, CodigoCancion, Subtotal)
--        VALUES (@NumeroFactura, @CodigoCancion, @Subtotal);
--    END
--    ELSE
--    BEGIN
--        PRINT 'Error: Factura o Canción no existente';
--    END
--END;

--EXEC InsertarDetalleVenta
--	@NumeroFactura = ,
--	@CodigoCancion = ,
--	@Subtotal = ;


----________________________Editar________________________----

--CREATE PROCEDURE EditarDetalleVenta
--    @IDDetalle INT,
--    @NumeroFactura INT,
--    @CodigoCancion INT,
--    @Subtotal DECIMAL(10,2)
--AS
--BEGIN
--    IF EXISTS (SELECT 1 FROM DetalleVenta WHERE IDDetalle = @IDDetalle)
--    BEGIN
--        UPDATE DetalleVenta
--        SET NumeroFactura = @NumeroFactura,
--            CodigoCancion = @CodigoCancion,
--            Subtotal = @Subtotal
--        WHERE IDDetalle = @IDDetalle;
--    END
--    ELSE
--    BEGIN
--        PRINT 'No se encontró el detalle de venta con ese ID.';
--    END
--END;


--EXEC EditarDetalleVenta 
--    @IDDetalle = ,
--    @NumeroFactura = ,
--    @CodigoCancion = ,
--    @Subtotal = ;

----________________________Eliminar________________________----

--CREATE PROCEDURE EliminarDetalleVenta
--    @IDDetalle INT
--AS
--BEGIN
--    IF EXISTS (SELECT 1 FROM DetalleVenta WHERE IDDetalle = @IDDetalle)
--    BEGIN
--        DELETE FROM DetalleVenta
--        WHERE IDDetalle = @IDDetalle;
--    END
--    ELSE
--    BEGIN
--        PRINT 'No se encontró el detalle de venta con ese ID.';
--    END
--END;

--EXEC EliminarDetalleVenta @IDDetalle = ;

----________________________Lista________________________----

--CREATE PROCEDURE ListarDetalleVenta
--AS
--BEGIN
--    SELECT IDDetalle, NumeroFactura, CodigoCancion, Subtotal
--    FROM DetalleVenta
--    ORDER BY IDDetalle;
--END;

--EXEC ListarDetalleVenta;

---- Generos
----________________________Insertar________________________----

--CREATE PROCEDURE InsertarGenero
--	@Descripcion VARCHAR(100)
--AS
--BEGIN
--    IF NOT EXISTS (SELECT 1 FROM Generos WHERE Descripcion = @Descripcion)
--    BEGIN
--        INSERT INTO Generos (Descripcion)
--        VALUES (@Descripcion);
--    END
--    ELSE
--    BEGIN
--        PRINT 'El género ya existe.';
--    END
--END;

--EXEC InsertarGenero @Descripcion = 'Rock';
----________________________Lista________________________----

--CREATE PROCEDURE ListaGeneros
--AS
--BEGIN
--	SELECT CodigoGenero, Descripcion
--	FROM Generos
--	ORDER BY Descripcion;
--END;

--EXEC ListaGeneros;

----________________________Editar________________________----

--CREATE PROCEDURE EditarGenero
--	@CodigoGenero		INT,
--	@NuevaDescripcion	VARCHAR(100)
--AS
--BEGIN

--	UPDATE Generos
--	SET Descripcion = @NuevaDescripcion
--	WHERE @CodigoGenero = @CodigoGenero;
--END;

--EXEC EditarGenero @CodigoGenero  = 2, @NuevaDescripcion = 'Pop Latino';

----________________________Eliminar________________________----

--CREATE PROCEDURE EliminarGenero
--    @CodigoGenero INT
--AS
--BEGIN
--    DELETE FROM Generos
--    WHERE CodigoGenero = @CodigoGenero;
--END;

--EXEC EliminarGenero @Codigogenero = ;


---- Usuarios (Josue)

---- ________________________Agregar Usuario________________________ -----
--CREATE PROCEDURE spAgregarUsuario
--	@NumeroIdentificacion	VARCHAR (30),
--	@Nombre					VARCHAR(30),
--	@Apellido				VARCHAR(50),
--	@Genero					NVARCHAR(10),
--	@CorreoElectronico		VARCHAR(255),
--	@TipoTarjeta			NVARCHAR(25),
--	@DineroDisponible		DECIMAL(10,2),
--	@NumeroTarjeta			VARCHAR(25),
--	@Contrasena				VARCHAR(30),
--	@Perfil					NVARCHAR(25)

--AS
--BEGIN
--    BEGIN TRY
--        BEGIN TRANSACTION;

--        INSERT INTO Usuarios 
--		( NumeroIdentificacion, Nombre, Apellido, Genero, CorreoElectronico, TipoTarjeta,
--		  DineroDisponible, NumeroTarjeta, Contrasena, Perfil )

--        VALUES 
--		( @NumeroIdentificacion, @Nombre, @Apellido, @Genero, @CorreoElectronico, @TipoTarjeta,
--		  @DineroDisponible, @NumeroTarjeta, @Contrasena, @Perfil );

--        COMMIT;
--    END TRY
--    BEGIN CATCH
--        ROLLBACK;
--        THROW;
--		DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
--        SELECT 
--            @ErrorMessage = ERROR_MESSAGE(),
--            @ErrorSeverity = ERROR_SEVERITY(),
--            @ErrorState = ERROR_STATE();
--        RAISERROR('Error al agregar al usuario: %s', @ErrorSeverity, @ErrorState, @ErrorMessage)
--    END CATCH
--END


------ ________________________Actualizar Usuario________________________ ----
--CREATE PROCEDURE spActualizarUsuario
--    @ID					  INT,
--    @NumeroIdentificacion VARCHAR(30),
--    @Nombre               VARCHAR(30),
--    @Apellido             VARCHAR(50),
--    @Genero               NVARCHAR(10),
--    @CorreoElectronico    VARCHAR(255),
--    @TipoTarjeta          NVARCHAR(25),
--    @DineroDisponible     DECIMAL(10,2),
--    @NumeroTarjeta        VARCHAR(25),
--    @Contrasena           VARCHAR(30),
--    @Perfil               NVARCHAR(25)
--AS
--BEGIN
--    BEGIN TRY
--        BEGIN TRANSACTION;

--        UPDATE Usuarios
--        SET 
--            NumeroIdentificacion = @NumeroIdentificacion,
--            Nombre = @Nombre,
--            Apellido = @Apellido,
--            Genero = @Genero,
--            CorreoElectronico = @CorreoElectronico,
--            TipoTarjeta = @TipoTarjeta,
--            DineroDisponible = @DineroDisponible,
--            NumeroTarjeta = @NumeroTarjeta,
--            Contrasena = @Contrasena,
--            Perfil = @Perfil
--        WHERE 
--            ID = @ID;

--        COMMIT;
--    END TRY
--    BEGIN CATCH
--        ROLLBACK;

--        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
--        SELECT 
--            @ErrorMessage = ERROR_MESSAGE(),
--            @ErrorSeverity = ERROR_SEVERITY(),
--            @ErrorState = ERROR_STATE();
--        RAISERROR('Error al actualizar el usuario: %s', @ErrorSeverity, @ErrorState, @ErrorMessage);
--    END CATCH
--END



------ ________________________Eliminar Usuario________________________ ----
--CREATE PROCEDURE spEliminarUsuario
--    @Id INT
--AS
--BEGIN
--    DELETE
--    FROM Usuarios
--    WHERE ID = @Id
--END


------ ________________________Mostrar Usuarios________________________ ----
--CREATE PROCEDURE spMostrarTodosUsuarios
   
--AS
--BEGIN
--    SELECT Id, NumeroIdentificacion, Nombre, Apellido, Genero, CorreoElectronico, TipoTarjeta,
--		   DineroDisponible, NumeroTarjeta, Contrasena, Perfil
--    FROM Usuarios
--END



------ ________________________Mostrar Usuario Por Id________________________ ----
--CREATE PROCEDURE spMostrarUsuariosPorId
--    @Id INT
--AS
--BEGIN
--    SELECT Id, NumeroIdentificacion, Nombre, Apellido, Genero, CorreoElectronico, TipoTarjeta,
--		   DineroDisponible, NumeroTarjeta, Contrasena, Perfil
--    FROM Usuarios
--    WHERE ID = @Id
--END



---- Ventas

---- ________________________Agregar Venta________________________ ----
--CREATE PROCEDURE spAgregarVenta
--    @IDUsuario         INT,
--    @FechaCompra       DATETIME,
--    @Total             DECIMAL(10,2),
--    @TipoPago          NVARCHAR(25)

--AS
--BEGIN
--    BEGIN TRY
--        BEGIN TRANSACTION;

--        INSERT INTO Ventas 
--		( IDUsuario, FechaCompra, Total, TipoPago)

--        VALUES 
--		( @IDUsuario, @FechaCompra, @Total, @TipoPago );

--        COMMIT;
--    END TRY
--    BEGIN CATCH
--        ROLLBACK;
--        THROW;
--		DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
--        SELECT 
--            @ErrorMessage = ERROR_MESSAGE(),
--            @ErrorSeverity = ERROR_SEVERITY(),
--            @ErrorState = ERROR_STATE();
--        RAISERROR('Error al realizar la venta: %s', @ErrorSeverity, @ErrorState, @ErrorMessage)
--    END CATCH
--END


---- ________________________Actualizar Venta________________________ ----
/* 
No implementada pues no considero que sea correcto poder actualizar una venta 
(alterar una factura)
*/

---- ________________________Eliminar Venta________________________ ----
/* 
No implementada pues no considero que sea correcto poder eliminar una venta 
(alterar una factura)
*/

---- ________________________Mostrar Ventas________________________ ----
--CREATE PROCEDURE spMostrarVentas
   
--AS
--BEGIN
--    SELECT NumeroFactura, IDUsuario, FechaCompra, Total, TipoPago
--    FROM Ventas
--END



---- ________________________Mostrar Venta Por Id________________________ ----
--CREATE PROCEDURE spMostrarVentaPorId
--    @NumeroFactura INT
--AS
--BEGIN
--    SELECT NumeroFactura, IDUsuario, FechaCompra, Total, TipoPago
--    FROM Ventas
--    WHERE NumeroFactura = @NumeroFactura
--END

----****************************** FIN PROCEDIMIENTOS ALMACENADOS *****************************



----******************************TRIGGER ******************************
---- Albumes (Freddy) ----
-- ________________________Insert________________________----
--CREATE TRIGGER tr_InsertAlbumes
--ON Albumes
--AFTER INSERT
--AS
--BEGIN
--    SET NOCOUNT ON;

--    INSERT INTO Auditoria (FechaRegistro, Usuario, Accion, TablaAfectada, IDRegistroAfectado, Detalles, IPUsuario)
--    SELECT 
--        GETDATE(),
--        i.NombreAlbum,      -- Nombre del álbum como identificador en la auditoría
--        'INSERT',
--        'Albumes',
--        i.CodigoAlbum,      -- ID del nuevo álbum
--        'Nuevo álbum insertado: ' + i.NombreAlbum,
--        SUSER_NAME()
--    FROM inserted i;
--END
--GO

-- ________________________Update________________________----
--CREATE TRIGGER tr_UpdateAlbumes
--ON Albumes
--AFTER UPDATE
--AS
--BEGIN
--    SET NOCOUNT ON;

--    INSERT INTO Auditoria (FechaRegistro, Usuario, Accion, TablaAfectada, IDRegistroAfectado, Detalles, IPUsuario)
--    SELECT 
--        GETDATE(),
--        i.NombreAlbum,
--        'UPDATE',
--        'Albumes',
--        i.CodigoAlbum,      -- ID del álbum actualizado
--        'Álbum actualizado: ' + i.NombreAlbum,
--        SUSER_NAME()
--    FROM inserted i;
--END
--GO

-- ________________________Delete________________________----
--CREATE TRIGGER tr_DeleteAlbumes
--ON Albumes
--AFTER DELETE
--AS
--BEGIN
--    SET NOCOUNT ON;

--    INSERT INTO Auditoria (FechaRegistro, Usuario, Accion, TablaAfectada, IDRegistroAfectado, Detalles, IPUsuario)
--    SELECT 
--        GETDATE(),
--        d.NombreAlbum,
--        'DELETE',
--        'Albumes',
--        d.CodigoAlbum,      -- ID del álbum eliminado
--        'Álbum eliminado: ' + d.NombreAlbum,
--        SUSER_NAME()
--    FROM deleted d;
--END
--GO

---- Artistas (Freddy) ----
-- ________________________Insert________________________----
--CREATE TRIGGER tr_InsertArtistas
--ON ARTISTAS
--AFTER INSERT
--AS
--BEGIN
--    -- Evita que se devuelva el recuento de filas afectadas
--    SET NOCOUNT ON;

--    -- Inserta el registro de auditoría para la nueva inserción
--    INSERT INTO Auditoria (FechaRegistro, Usuario, Accion, TablaAfectada, IDRegistroAfectado, Detalles, IPUsuario)
--    SELECT 
--        GETDATE(),          -- Fecha y hora actual de la operación
--        i.NombreArtistico,  -- Nombre del artista como 'Usuario' que realiza la acción
--        'INSERT',           -- Acción realizada
--        'Artistas',         -- Tabla afectada
--        i.CodigoArtista,    -- ID del nuevo registro insertado
--        'Nuevo artista insertado: ' + i.NombreArtistico, -- Detalles de la operación
--        SUSER_NAME()        -- Nombre del usuario de la sesión de SQL Server
----    FROM inserted i;        -- 'inserted' es una tabla virtual que contiene las nuevas filas
--END
--GO

-- ________________________Update________________________----
--CREATE TRIGGER tr_UpdateArtistas
--ON ARTISTAS
--AFTER UPDATE
--AS
--BEGIN
--    SET NOCOUNT ON;

--    -- Inserta el registro de auditoría para la actualización
--    INSERT INTO Auditoria (FechaRegistro, Usuario, Accion, TablaAfectada, IDRegistroAfectado, Detalles, IPUsuario)
--    SELECT 
--        GETDATE(),
--        i.NombreArtistico,
--        'UPDATE',
--        'Artistas',
--        i.CodigoArtista,    -- ID del registro actualizado
--        'Artista actualizado: ' + i.NombreArtistico,
--        SUSER_NAME()
--    FROM inserted i;
--END
--GO

-- ________________________Delete________________________----
--CREATE TRIGGER tr_DeleteArtistas
--ON ARTISTAS
--AFTER DELETE
--AS
--BEGIN
--    SET NOCOUNT ON;
--
--    -- Inserta el registro de auditoría para la eliminación
--    INSERT INTO Auditoria (FechaRegistro, Usuario, Accion, TablaAfectada, IDRegistroAfectado, Detalles, IPUsuario)
--    SELECT 
--        GETDATE(),
--        d.NombreArtistico,  -- Se obtiene el nombre de la tabla 'deleted'
--        'DELETE',
--        'Artistas',
--        d.CodigoArtista,    -- ID del registro eliminado
--        'Artista eliminado: ' + d.NombreArtistico,
--        SUSER_NAME()
--    FROM deleted d;         -- 'deleted' es una tabla virtual que contiene las filas eliminadas
--END
--GO



---- Auditoria (Fabio)
---- Canciones
----                         ***** TRIGGER FABIO. INICIO*****


----							Canciones

---- ________________________Insert________________________ ----

--CREATE TRIGGER tr_Canciones_Insert
--ON Canciones
--AFTER INSERT
--AS
--BEGIN
--    SET NOCOUNT ON;

--    INSERT INTO Auditoria (FechaRegistro, Usuario, Accion, TablaAfectada, IDRegistroAfectado, Detalles, IPUsuario)
--    SELECT 
--        GETDATE(),
--        'Sistema',
--        'INSERT',
--        'Canciones',
--        i.CodigoCancion,
--        'Canción agregada',
--        'Servidor SQL'
--    FROM inserted i;
--END


---- ________________________Update________________________ ----

--CREATE TRIGGER tr_Canciones_Update
--ON Canciones
--AFTER UPDATE
--AS
--BEGIN
--    SET NOCOUNT ON;

--    INSERT INTO Auditoria (FechaRegistro, Usuario, Accion, TablaAfectada, IDRegistroAfectado, Detalles, IPUsuario)
--    SELECT 
--        GETDATE(),
--        'Sistema',
--        'UPDATE',
--        'Canciones',
--        i.CodigoCancion,
--        'Canción actualizada',
--        'Servidor SQL'
--    FROM inserted i
--    JOIN deleted d ON i.CodigoCancion = d.CodigoCancion;
--END


---- ________________________Delete________________________ ----

--CREATE TRIGGER tr_Canciones_Delete
--ON Canciones
--AFTER DELETE
--AS
--BEGIN
--    SET NOCOUNT ON;

--    INSERT INTO Auditoria (FechaRegistro, Usuario, Accion, TablaAfectada, IDRegistroAfectado, Detalles, IPUsuario)
--    SELECT 
--        GETDATE(),
--        'Sistema',
--        'DELETE',
--        'Canciones',
--        d.CodigoCancion,
--        'Canción eliminada',
--        'Servidor SQL'
--    FROM deleted d;
--END


----							Auditoria 

---- ________________________Nota Importante________________________ ----
--No se deben crear triggers sobre la tabla Auditoria ya que su función es 
--precisamente registrar acciones hechas sobre otras tablas. Crear triggers 
--sobre esta tabla causaría recursividad infinita o errores lógicos.

--Por ejemplo, si se hiciera un INSERT en Auditoria y eso a su vez activara un 
--trigger para registrar el INSERT, se generaría un bucle.



----						 ***** TRIGGER FABIO. FIN*****
---- Detalles (Raquel)
----________________________Insertar________________________----

--CREATE TRIGGER trg_InsertDetalleVenta
--ON DetalleVenta
--AFTER INSERT
--AS
--BEGIN
--    INSERT INTO AuditoriaDetalleVenta (NumeroFactura, CodigoCancion, Subtotal, FechaInsert, UsuarioInserto)
--    SELECT 
--        i.NumeroFactura,
--        i.CodigoCancion,
--        i.Subtotal,
--        GETDATE(),
--        SYSTEM_USER
--    FROM inserted i;
--END;

----________________________Actualizar________________________----

--CREATE TRIGGER tr_ActualizarTotalVenta_DespuesInsertar
--ON DetalleVenta
--AFTER INSERT
--AS
--BEGIN
--    UPDATE v
--    SET v.Total = (
--        SELECT SUM(Subtotal)
--        FROM DetalleVenta dv
--        WHERE dv.NumeroFactura = v.NumeroFactura
--    )
--    FROM Ventas v
--    JOIN inserted i ON v.NumeroFactura = i.NumeroFactura;
--END;


----________________________Validar SubTotal no sea Menor a cero________________________----

--CREATE TRIGGER tr_ValidarSubtotal_MayorCero
--ON DetalleVenta
--INSTEAD OF INSERT
--AS
--BEGIN
--    IF EXISTS (SELECT 1 FROM inserted WHERE Subtotal <= 0)
--    BEGIN
--        RAISERROR('El subtotal debe ser mayor a cero.', 16, 1);
--        ROLLBACK;
--        RETURN;
--    END

--    INSERT INTO DetalleVenta (NumeroFactura, CodigoCancion, Subtotal)
--    SELECT NumeroFactura, CodigoCancion, Subtotal FROM inserted;
--END;



---- Generos
----________________________No Exista Duplicados________________________----

--CREATE TRIGGER trg_PreventDuplicateGenero
--ON Generos
--INSTEAD OF INSERT
--AS
--BEGIN
--    IF EXISTS (
--        SELECT 1
--        FROM inserted i
--        JOIN Generos g ON LOWER(i.Descripcion) = LOWER(g.Descripcion)
--    )
--    BEGIN
--        RAISERROR('Ya existe un género con esa descripción.', 16, 1);
--        ROLLBACK;
--        RETURN;
--    END

--    INSERT INTO Generos (Descripcion)
--    SELECT Descripcion
--    FROM inserted;
--END;

----________________________Eliminar________________________

--CREATE TRIGGER tr_EvitarEliminarGeneroConCanciones
--ON Generos
--INSTEAD OF DELETE
--AS
--BEGIN
--    -- Verificar si hay canciones asociadas al género que se intenta eliminar
--    IF EXISTS (
--        SELECT 1
--        FROM deleted d
--        JOIN Canciones c ON c.CodigoGenero = d.CodigoGenero
--    )
--    BEGIN
--        RAISERROR('No se puede eliminar el género porque está asociado a una o más canciones.', 16, 1);
--        ROLLBACK;
--        RETURN;
--    END

--    DELETE FROM Generos
--    WHERE CodigoGenero IN (SELECT CodigoGenero FROM deleted);
--END;


---- Usuarios (Josue)

---- ________________________Insert________________________ ----
--CREATE TRIGGER trInsertUsuarios
--ON Usuarios
--AFTER INSERT
--AS
--BEGIN
--    SET NOCOUNT ON;

--    INSERT INTO Auditoria (FechaRegistro, Usuario, Accion, TablaAfectada, IDRegistroAfectado, Detalles, IPUsuario)
--    SELECT 
--        GETDATE(),
--        Nombre,
--        'INSERT',
--        'Usuarios',
--        ID,
--        'Nuevo usuario insertado',
--        Perfil
--    FROM inserted;
--END

------ ________________________Update________________________ ----
--CREATE TRIGGER tr_Usuarios_Update
--ON Usuarios
--AFTER UPDATE
--AS
--BEGIN
--    SET NOCOUNT ON;

--    INSERT INTO Auditoria (FechaRegistro, Usuario, Accion, TablaAfectada, IDRegistroAfectado, Detalles, IPUsuario)
--    SELECT 
--        GETDATE(),
--        i.Nombre,
--        'UPDATE',
--        'Usuarios',
--        i.ID,
--        'Usuario actualizado',
--        i.Perfil
--    FROM inserted i
--    JOIN deleted d ON i.ID = d.ID;
--END
------ ________________________Delete________________________ ----
--CREATE TRIGGER tr_Usuarios_Delete
--ON Usuarios
--AFTER DELETE
--AS
--BEGIN
--    SET NOCOUNT ON;

--    INSERT INTO Auditoria (FechaRegistro, Usuario, Accion, TablaAfectada, IDRegistroAfectado, Detalles, IPUsuario)
--    SELECT 
--        GETDATE(),
--        Nombre,
--        'DELETE',
--        'Usuarios',
--        ID,
--        'Usuario eliminado',
--        Perfil
--    FROM deleted;
--END

------ Ventas

------ ________________________Insert________________________ ----
--CREATE TRIGGER trInsertVenta
--ON Ventas
--AFTER INSERT
--AS
--BEGIN
--    SET NOCOUNT ON;

--    INSERT INTO Auditoria (FechaRegistro, Usuario, Accion, TablaAfectada, IDRegistroAfectado, Detalles, IPUsuario)
--    SELECT 
--        GETDATE(),
--        u.ID,
--        'INSERT',
--        'Ventas',
--        i.NumeroFactura,
--        'Nueva venta registrada',
--        'Sistema de cobros'
--    FROM inserted i
--	INNER JOIN Usuarios u ON u.ID = i.IDUsuario;
--END
----****************************** FIN TRIGGER ******************************
