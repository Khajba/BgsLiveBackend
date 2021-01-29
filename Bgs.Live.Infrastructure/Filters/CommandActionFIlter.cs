using Bgs.Live.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Bgs.Live.Infrastructure.Filters
{
    public class CommandActionFIlter : ActionFilterAttribute
    {

        public CommandActionFIlter()
        {

        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {

            var arg = context.ActionArguments.FirstOrDefault(a => a.Value is CommandModel);
            if(arg.Value == null)
            {
                return;
            }
            var commandModel = (CommandModel)arg.Value;

            var refer = context.HttpContext.Request.Headers["Referer"].ToString();
            var id = context.HttpContext.Request.Headers["Corelation-Id"].ToString();
            if(refer != null && id != null)
            {
                commandModel.Referrer = refer;
                commandModel.CorrelationId = id;
            }
            



        }



    }
}
