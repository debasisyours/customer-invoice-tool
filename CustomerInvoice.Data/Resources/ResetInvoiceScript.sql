DELETE FROM InvoiceDetail WHERE InvoiceId>@InvoiceNumber
DELETE FROM Invoice WHERE Id>@InvoiceNumber

DBCC CHECKIDENT('Invoice', RESEED, @InvoiceNumber)