CREATE PROCEDURE usp_Invoice_History
	@CompanyId	INT
AS
BEGIN
	CREATE TABLE #InvoiceHistory
		(
			[Select]		BIT,
			[Id]			BIGINT,
			[InvoiceNumber]	VARCHAR(50),
			[InvoiceDate]	DATETIME,
			[ClientCode]	VARCHAR(50),
			[ClientName]	VARCHAR(100),
			[CustomerCode]	VARCHAR(50),
			[CustomerName]	VARCHAR(100),
			[ChargeName]	VARCHAR(100),
			[WeeklyRate]	MONEY,
			[Days]			INT,
			[SubTotal]		MONEY,
			[ExtraHead]		VARCHAR(100),
			[ExtraAmount]	MONEY,
			[LessHead]		VARCHAR(100),
			[LessAmount]	MONEY,
			[NetAmount]		MONEY,
			[ChargePeriod]	VARCHAR(100),
			[InvoiceType]	VARCHAR(50),
			[Deleted]		BIT,
			[MultiMonth]	BIT
		)

	-- Processing for Invoices
	INSERT #InvoiceHistory
		([Select],
			[Id],
			[InvoiceNumber],
			[InvoiceDate],
			[ClientCode],
			[ClientName],
			[Deleted],
			[MultiMonth],
			[NetAmount],
			[InvoiceType])
		SELECT
			0 AS [SELECT]
		   ,I.ID
		   ,InvoiceNumber
		   ,InvoiceDate
		   ,ISNULL(C.Code, '') AS ClientCode
		   ,ISNULL(C.[Name], '') AS ClientName
		   ,Deleted
		   ,MultiMonth
		   ,NetAmount
		   ,'SI' AS InvoiceType
		FROM Invoice I
			 LEFT JOIN Client C ON I.ClientID = C.ID AND I.CompanyId = C.CompanyId
		WHERE I.Printed = 1
		  AND I.CompanyId = @CompanyId

	UPDATE IH
	   SET IH.CustomerCode = ISNULL(C.Code, ''),
		   IH.CustomerName = ISNULL(C.[Name], ''),
		   IH.Days = ID.Days,
		   IH.ChargePeriod = REPLACE(CONVERT(VARCHAR(12), I.StartDate, 110), '-', '/') + ' to ' + REPLACE(CONVERT(VARCHAR(12), I.EndDate, 110), '-', '/') + ' - ' + CONVERT(VARCHAR(5),(ID.Days / 7)) + ' weeks'
	  FROM #InvoiceHistory IH
		   INNER JOIN Invoice I ON IH.Id = I.ID
		   LEFT JOIN InvoiceDetail ID ON IH.Id = ID.InvoiceID
		   LEFT JOIN Customer C ON ID.CustomerID = C.ID

	UPDATE IH
	   SET IH.WeeklyRate = WeeklySum.WeekSum,
		   IH.ExtraAmount = WeeklySum.ExtraSum,
		   IH.LessAmount = WeeklySum.LessSum,
		   IH.SubTotal = WeeklySum.SubTotalSum
	  FROM #InvoiceHistory IH
	  LEFT JOIN(
			SELECT 
				InvoiceId, 
				SUM(WeeklyRate) AS WeekSum, 
				SUM(ExtraAmount) AS ExtraSum, 
				SUM(LessAmount) AS LessSum,
				SUM(SubTotal) AS SubTotalSum
			FROM InvoiceDetail ID
			GROUP BY InvoiceID
			) WeeklySum ON IH.Id = WeeklySum.InvoiceId
	

	UPDATE IH
	   SET ChargeName = STUFF((SELECT '+ ' + Name 
							   FROM ChargeHead 
							   WHERE Id IN
								(SELECT ChargeHeadID FROM InvoiceDetail WHERE InvoiceID = IH.Id) 
							   FOR XML PATH('')),1,1, '')
	FROM #InvoiceHistory IH

	UPDATE IH
	   SET ExtraHead = STUFF((SELECT ', ' + ExtraHead 
							   FROM InvoiceDetail WHERE InvoiceDetail.InvoiceID = IH.Id
							   FOR XML PATH('')),1,1, '')
	FROM #InvoiceHistory IH

	UPDATE IH
	   SET LessHead = STUFF((SELECT ', ' + LessHead 
							   FROM InvoiceDetail WHERE InvoiceDetail.InvoiceID = IH.Id
							   FOR XML PATH('')),1,1, '')
	FROM #InvoiceHistory IH

	-- Processing for Credit Notes

	INSERT #InvoiceHistory
		(
			[Select],
			[Id],
			[InvoiceNumber],
			[InvoiceDate],
			[ClientCode],
			[ClientName],
			[CustomerCode],
			[CustomerName],
			[WeeklyRate],
			[Days],
			[SubTotal],
			[ExtraHead],
			[ExtraAmount],
			[LessHead],
			[LessAmount],
			[NetAmount],
			[ChargePeriod],
			[InvoiceType],
			[Deleted],
			[MultiMonth]
		)
	SELECT
		0 AS [Select],
		CreditNote.ID,
		CreditNote.TransactionNumber AS InvoiceNumber,
		CreditNote.TransactionDate AS InvoiceDate,
		ISNULL(C.Code, '') AS ClientCode,
		ISNULL(C.Name, '') AS ClientName,
		ISNULL(CM.Code, '') AS CustomerCode,
		ISNULL(CM.Name, '') AS CustomerName,
		0,
		0,
		CreditNote.Amount AS SubTotal,
		'' AS ExtraHead,
		0 AS ExtraAmount,
		'' AS LessHead,
		0 AS LessAmount,
		CreditNote.Amount AS NetAmount,
		'' AS ChargePeriod,
		'SC' AS InvoiceType,
		0 AS Deleted,
		0 AS MultiMonth
	FROM CreditNote
		 LEFT JOIN Customer CM ON CreditNote.CustomerId = CM.ID
		 LEFT JOIN Client C ON CreditNote.ClientId = C.ID

	SELECT [Select],
			[Id],
			[InvoiceNumber],
			[InvoiceDate],
			[ClientCode],
			[ClientName],
			[CustomerCode],
			[CustomerName],
			[ChargeName],
			[WeeklyRate],
			[Days],
			[SubTotal],
			IIF(TRIM([ExtraHead]) = ',', '', [ExtraHead]) AS ExtraHead,
			[ExtraAmount],
			IIF(TRIM([LessHead]) = ',', '', [LessHead]) AS LessHead,
			[LessAmount],
			[NetAmount],
			[ChargePeriod],
			[InvoiceType],
			[Deleted],
			[MultiMonth] 
	FROM #InvoiceHistory
	ORDER BY InvoiceType, Id, InvoiceDate 
END