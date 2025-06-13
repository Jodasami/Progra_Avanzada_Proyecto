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

---- Albumes (Freddy)
---- Artistas

---- Auditoria (Fabio)
---- Canciones

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
---- Albumes (Freddy)
---- Artistas

---- Auditoria (Fabio)
---- Canciones

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
