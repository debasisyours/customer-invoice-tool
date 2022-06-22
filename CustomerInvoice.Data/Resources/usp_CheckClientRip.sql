-- =============================================
-- Author	  :	<Debasis C.>
-- Create date: <01-Nov-2019>
-- Description:	<Checks whether to generate invoice for a customer if any of the client(s) are marked RIP>
-- Usage	  : exec usp_CheckClientRip 1, '30-Oct-2019'
-- =============================================
CREATE PROCEDURE usp_CheckClientRip
	@CompanyId	INT,
	@InvoiceEndDate	DATETIME
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		 C.Id
	FROM dbo.BreakDown B WITH(NOLOCK)
	JOIN dbo.BreakDownDetail BD WITH(NOLOCK) ON B.ID = BD.BreakDownID
	LEFT JOIN dbo.Client C WITH(NOLOCK) ON B.ClientID = C.ID AND B.CompanyId = C.CompanyId
	LEFT JOIN dbo.Customer CS WITH(NOLOCK) ON BD.CustomerID = CS.ID AND B.CompanyId = CS.CompanyId
	WHERE B.CompanyId = @CompanyId
	  AND BD.IsActive = 1
	  AND CAST(ISNULL(C.Rip, '1901-01-01') AS DATETIME) BETWEEN CAST('2001-01-01' AS DATETIME) AND @InvoiceEndDate
END