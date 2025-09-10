create database Facturacion_Db
use Facturacion_Db

create table Articulos
(
id_articulo int identity(1,1),
nombre varchar(MAX),
pre_unitario int,
stock int,
constraint Pk_articulo primary key(id_articulo)
)

create table Clientes
(
id_cliente int identity(1,1),
nombre varchar(MAX),
telefono int,
constraint Pk_cliente primary key(id_cliente)
)

create table FormasPago
(
id_forma_pago int identity(1,1),
nombre varchar(MAX),
constraint Pk_forma_pago primary key (id_forma_pago)
)

create table Factura
(
id_factura int identity(1,1),
fecha datetime,
id_forma_pago int,
id_cliente int,
constraint Pk_factura primary key(id_factura),
constraint Fk_factura_Fp foreign key(id_forma_pago)
references FormasPago(id_forma_pago),
constraint Fk_factura_C foreign key(id_cliente)
references Clientes(id_cliente)
)

create table DetalleFacturas
(
id_detalle int identity(1,1),
id_articulo int,
cantidad int,
id_factura int,
constraint Pk_DetalleFacturas primary key(id_detalle),
constraint Fk_DetalleFacturas_A foreign key(id_articulo)
references Articulos(id_articulo),
constraint Fk_DetalleFacturas_F foreign key(id_factura)
references Factura(id_factura)
)

USE Facturacion_Db
GO

-- 1. Procedimiento para insertar una factura
CREATE PROCEDURE Sp_INSERT_BILL
    @fecha DATETIME,
    @id_forma_pago INT,
    @id_cliente INT,
    @id_factura INT OUTPUT
AS
BEGIN
    INSERT INTO Factura (fecha, id_forma_pago, id_cliente)
    VALUES (@fecha, @id_forma_pago, @id_cliente)
    
    SET @id_factura = SCOPE_IDENTITY()
END
GO

-- 2. Procedimiento para insertar un detalle de factura
CREATE PROCEDURE Sp_INSERT_BILL_ITEM
    @id_factura INT,
    @id_articulo INT,
    @cantidad INT
AS
BEGIN
    DECLARE @precio_unitario INT
    
    -- Obtener el precio unitario del artículo
    SELECT @precio_unitario = pre_unitario 
    FROM Articulos 
    WHERE id_articulo = @id_articulo
    
    -- Insertar el detalle
    INSERT INTO DetalleFacturas (id_factura, id_articulo, cantidad)
    VALUES (@id_factura, @id_articulo, @cantidad)
    
    -- Actualizar el stock del artículo
    UPDATE Articulos 
    SET stock = stock - @cantidad 
    WHERE id_articulo = @id_articulo
END
GO

-- 3. Procedimiento para obtener todas las facturas
CREATE PROCEDURE Sp_GET_ALL_BILLS
AS
BEGIN
    SELECT 
        f.id_factura,
        f.fecha,
        f.id_forma_pago,
        fp.nombre as forma_pago,
        f.id_cliente,
        c.nombre as cliente_nombre,
        c.telefono
    FROM Factura f
    INNER JOIN FormasPago fp ON f.id_forma_pago = fp.id_forma_pago
    INNER JOIN Clientes c ON f.id_cliente = c.id_cliente
    ORDER BY f.fecha DESC
END
GO

-- 4. Procedimiento para obtener una factura por ID
CREATE PROCEDURE Sp_GET_BILL_BY_ID
    @id_factura INT
AS
BEGIN
    -- Obtener información de la factura
    SELECT 
        f.id_factura,
        f.fecha,
        f.id_forma_pago,
        fp.nombre as forma_pago,
        f.id_cliente,
        c.nombre as cliente_nombre,
        c.telefono
    FROM Factura f
    INNER JOIN FormasPago fp ON f.id_forma_pago = fp.id_forma_pago
    INNER JOIN Clientes c ON f.id_cliente = c.id_cliente
    WHERE f.id_factura = @id_factura
    
    -- Obtener los detalles de la factura
    SELECT 
        df.id_detalle,
        df.id_articulo,
        a.nombre as articulo_nombre,
        a.pre_unitario as precio_unitario,
        df.cantidad,
        (a.pre_unitario * df.cantidad) as subtotal
    FROM DetalleFacturas df
    INNER JOIN Articulos a ON df.id_articulo = a.id_articulo
    WHERE df.id_factura = @id_factura
END
GO

-- 5. Procedimiento para obtener los detalles de una factura
CREATE PROCEDURE Sp_GET_BILL_DETAILS
    @id_factura INT
AS
BEGIN
    SELECT 
        df.id_detalle,
        df.id_articulo,
        a.nombre as articulo_nombre,
        a.pre_unitario as precio_unitario,
        df.cantidad,
        (a.pre_unitario * df.cantidad) as subtotal
    FROM DetalleFacturas df
    INNER JOIN Articulos a ON df.id_articulo = a.id_articulo
    WHERE df.id_factura = @id_factura
END
GO

-- 6. Procedimiento para obtener un artículo por ID
CREATE PROCEDURE Sp_GET_PRODUCT_BY_ID
    @Id INT
AS
BEGIN
    SELECT 
        id_articulo as Id,
        nombre as Name,
        pre_unitario as Price,
        stock as Stock
    FROM Articulos
    WHERE id_articulo = @Id
END
GO

-- 7. Procedimiento para obtener todos los artículos
CREATE PROCEDURE Sp_GET_ALL_PRODUCTS
AS
BEGIN
    SELECT 
        id_articulo as Id,
        nombre as Name,
        pre_unitario as Price,
        stock as Stock
    FROM Articulos
    ORDER BY nombre
END
GO

-- 8. Procedimiento para obtener un cliente por ID
CREATE PROCEDURE Sp_GET_CLIENT_BY_ID
    @Id INT
AS
BEGIN
    SELECT 
        id_cliente as id,
        nombre as name,
        telefono as phone
    FROM Clientes
    WHERE id_cliente = @Id
END
GO

-- 9. Procedimiento para insertar un cliente
CREATE PROCEDURE Sp_INSERT_CLIENT
    @Name VARCHAR(MAX),
    @Phone INT,
    @id INT OUTPUT
AS
BEGIN
    INSERT INTO Clientes (nombre, telefono)
    VALUES (@Name, @Phone)
    
    SET @id = SCOPE_IDENTITY()
END
GO

-- 10. Procedimiento para obtener una forma de pago por ID
CREATE PROCEDURE Sp_GET_PAYMENT_BY_ID
    @id INT
AS
BEGIN
    SELECT 
        id_forma_pago as Id,
        nombre as Method
    FROM FormasPago
    WHERE id_forma_pago = @id
END
GO

-- 11. Procedimiento para insertar una forma de pago
CREATE PROCEDURE Sp_INSERT_PAYMENT
    @Method VARCHAR(MAX),
    @id INT OUTPUT
AS
BEGIN
    INSERT INTO FormasPago (nombre)
    VALUES (@Method)
    
    SET @id = SCOPE_IDENTITY()
END
GO

-- 12. Procedimiento para obtener todas las formas de pago
CREATE PROCEDURE Sp_GET_ALL_PAYMENTS
AS
BEGIN
    SELECT 
        id_forma_pago as Id,
        nombre as Method
    FROM FormasPago
    ORDER BY nombre
END
GO

-- 13. Procedimiento para verificar stock antes de agregar un artículo a la factura
CREATE PROCEDURE Sp_CHECK_STOCK
    @id_articulo INT,
    @cantidad INT,
    @HayStock BIT OUTPUT
AS
BEGIN
    DECLARE @StockActual INT
    
    SELECT @StockActual = stock 
    FROM Articulos 
    WHERE id_articulo = @id_articulo
    
    IF @StockActual >= @cantidad
        SET @HayStock = 1
    ELSE
        SET @HayStock = 0
END
GO

-- 14. Procedimiento para actualizar una factura
CREATE PROCEDURE Sp_UPDATE_BILL
    @id_factura INT,
    @fecha DATETIME,
    @id_forma_pago INT,
    @id_cliente INT
AS
BEGIN
    UPDATE Factura 
    SET fecha = @fecha,
        id_forma_pago = @id_forma_pago,
        id_cliente = @id_cliente
    WHERE id_factura = @id_factura
END
GO

-- 15. Procedimiento para eliminar una factura
CREATE PROCEDURE Sp_DELETE_BILL
    @id_factura INT
AS
BEGIN
    -- Primero eliminar los detalles
    DELETE FROM DetalleFacturas 
    WHERE id_factura = @id_factura
    
    -- Luego eliminar la factura
    DELETE FROM Factura 
    WHERE id_factura = @id_factura
END
GO

-- 16. Procedimiento para obtener el total de una factura
CREATE PROCEDURE Sp_GET_BILL_TOTAL
    @id_factura INT,
    @Total DECIMAL(18,2) OUTPUT
AS
BEGIN
    SELECT @Total = SUM(a.pre_unitario * df.cantidad)
    FROM DetalleFacturas df
    INNER JOIN Articulos a ON df.id_articulo = a.id_articulo
    WHERE df.id_factura = @id_factura
END
GO

--INSERTS
-- Insertar formas de pago
INSERT INTO FormasPago (nombre) VALUES 
('Efectivo'),
('Tarjeta de Crédito'),
('Tarjeta de Débito'),
('Transferencia Bancaria'),
('Cheque')

-- Insertar clientes
INSERT INTO Clientes (nombre, telefono) VALUES 
('Juan Pérez', 123456789),
('María García', 987654321),
('Carlos López', 555123456),
('Ana Martínez', 444987654),
('Pedro Rodríguez', 333555789)

-- Insertar artículos
INSERT INTO Articulos (nombre, pre_unitario, stock) VALUES 
('Laptop HP 15"', 15000, 25),
('Mouse Inalámbrico', 500, 100),
('Teclado Mecánico', 1200, 50),
('Monitor 24" LED', 8000, 30),
('Impresora Laser', 4500, 15),
('Tablet Samsung', 7000, 20),
('Auriculares Bluetooth', 800, 75),
('Disco Duro Externo 1TB', 2000, 40),
('Memoria USB 32GB', 300, 200),
('Router WiFi', 1500, 35)

-- Insertar facturas
INSERT INTO Factura (fecha, id_forma_pago, id_cliente) VALUES 
('2024-01-15 10:30:00', 2, 1),
('2024-01-16 14:45:00', 1, 3),
('2024-01-17 16:20:00', 3, 2),
('2024-01-18 11:15:00', 2, 4),
('2024-01-19 09:30:00', 4, 5)

-- Insertar detalles de facturas
-- Factura 1
INSERT INTO DetalleFacturas (id_factura, id_articulo, cantidad) VALUES 
(1, 1, 1),  -- Laptop
(1, 2, 2),  -- Mouse
(1, 3, 1)   -- Teclado

-- Factura 2
INSERT INTO DetalleFacturas (id_factura, id_articulo, cantidad) VALUES 
(2, 4, 2),  -- Monitor
(2, 5, 1)   -- Impresora

-- Factura 3
INSERT INTO DetalleFacturas (id_factura, id_articulo, cantidad) VALUES 
(3, 6, 1),  -- Tablet
(3, 7, 3),  -- Auriculares
(3, 8, 1)   -- Disco Duro

-- Factura 4
INSERT INTO DetalleFacturas (id_factura, id_articulo, cantidad) VALUES 
(4, 9, 5),  -- Memoria USB
(4, 10, 1)  -- Router

-- Factura 5
INSERT INTO DetalleFacturas (id_factura, id_articulo, cantidad) VALUES 
(5, 2, 1),  -- Mouse
(5, 7, 2),  -- Auriculares
(5, 9, 3)   -- Memoria USB
