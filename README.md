# JGP.Core

Provide receipts for your service actions.

Executed some CRUD against the database? ActionReceipt.
Execution succeeded? ActionReceipt.
Execution failed? ActionReceipt.

Example usage:

        public async Task<ActionReceipt> PerformSomeCRUD(SomeClass someClass)
        {
        	try
        	{
        		var target = await FindSomethingAsync(someClass.SomeProperty);
        		if (target == null)
        		{
        			return ActionReceipt.GetNotFoundReceipt("Couldn't find target.");
        		}
        
        		target.Update(someClass);
        		var affectedTotal = await SaveSomethingAsync();
        		return ActionReceipt.GetSuccessReceipt(affectedTotal);
        	}
        	catch (Exception ex)
        	{
        		_logger.LogError(ex, ...);
        		return ActionReceipt.GetErrorReceipt(ex);
        	}
        }

Now with handy-dandy ModelStateDictionary support!

Add errors to your ModelStateDictionary and show your users how they messed up.
Or.. If you're not a RockStar 10x dev (despite your sweet triple monitor setup and mechanical keyboard) - make debugging issues a little easier.

        ModelState.AddModelValidationErrors(actionReceipt);
