
DECLARE @RC int
DECLARE @fileName varchar(25)
DECLARE @pin_conv_run_time datetime
DECLARE @pin_conv_run_id varchar(20)

SET @fileName = 'PADDRSTB.TXT';
SET @pin_conv_run_time = '3/23/2017';
SET @pin_conv_run_id = 'RUN01';

EXECUTE @RC = [dbo].[DCConv_Refresh_Address] 
   @fileName
  ,@pin_conv_run_time
  ,@pin_conv_run_id

SET @fileName = 'PENROLTB.TXT';
EXECUTE @RC = [dbo].[DCConv_Refresh_Enroll] 
   @fileName
  ,@pin_conv_run_time
  ,@pin_conv_run_id


SET @fileName = 'PLICNSTB.TXT';
EXECUTE @RC = [dbo].[DCConv_Refresh_License] 
   @fileName
  ,@pin_conv_run_time
  ,@pin_conv_run_id


SET @fileName = 'PSPECLTB.TXT';
EXECUTE @RC = [dbo].[DCConv_Refresh_Specialty] 
   @fileName
  ,@pin_conv_run_time
  ,@pin_conv_run_id


SET @fileName = 'PTAXIDTB.TXT';
EXECUTE @RC = [dbo].[DCConv_Refresh_TaxId] 
   @fileName
  ,@pin_conv_run_time
  ,@pin_conv_run_id


SET @fileName = 'PTAXONTB.TXT';
EXECUTE @RC = [dbo].[DCConv_Refresh_Taxonomy] 
   @fileName
  ,@pin_conv_run_time
  ,@pin_conv_run_id



