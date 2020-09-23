    	 EXEC msdb.dbo.sp_send_dbmail 
		 @profile_name ='XXX',
		 @recipients='test@test.de',
		 @subject = 'Testmail',
		 @body = 'Das ist ein Test',
		 @body_format = 'TEXT'; 