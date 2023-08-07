using CalculatorService.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Serilog;
using System.Collections.Generic;
using CalculatorService.Server.Models;
using Microsoft.AspNetCore.SignalR;

namespace CalculatorService.Server.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalculatorController : ControllerBase
    {
        private readonly ICalculator _calculator;
        private readonly IErrors _errors;
        public CalculatorController(ICalculator calculator, IErrors errors)
        {
            _calculator = calculator;
            _errors = errors;
        }


        /// <summary>
        /// Returns the sum of multiple numbers 
        /// </summary>
        /// <param name="operands">A list of numbers</param>
        /// <param name="X-Evi-Tracking-Id">optional: id tracking for save the current operand in the database</param>      
        /// <response code="200">Return the result of addition</response>
        /// <response code="400">At least two numeric operands are required.</response>
        /// <response code="500">Internal Error</response> 
        [HttpPost("add")]
        [MapToApiVersion("1.0")]
        [Produces("application/json")]
        public async Task<ActionResult> AddAsync([FromBody] Add additions)
        {
            try
            {
                
                if (additions.MainOperations == null || additions.MainOperations.Count < 2)
                {
                    Log.Information("Method: " + MethodBase.GetCurrentMethod().DeclaringType.FullName + "At least two numeric operands are required.");
                    return BadRequest(_errors.GetErrorMessage(400));
                    
                }
                additions.Result = await _calculator.GetAddResult(additions);

                if(Request.Headers["X-Evi-Tracking-Id"].Count > 0) 
                {
                    await _calculator.SaveTrackingId(Request.Headers["X-Evi-Tracking-Id"], additions, null);
                    Request.Headers.Remove("X-Evi-Tracking-Id");

                }
                return Ok(new { Sum = additions.Result } );
            }
            catch (Exception ex)
            {
                //return StatusCode(500, "Error in the application" + ex);
                Log.Error(ex,"Method: " + MethodBase.GetCurrentMethod().DeclaringType.FullName + " Error: " + ex.Message);
                return StatusCode(500, _errors.GetErrorMessage(500));
            } 
        }

        /// <summary>
        /// Returns the subtract of multiple numbers
        /// </summary>
        /// <param name="operands">A list of numbers</param>
        /// <param name="X-Evi-Tracking-Id">optional: id tracking for save the current operand in the database</param>      
        /// <response code="200">Return the result of subtract</response>
        /// <response code="400">At least two numeric operands are required.</response>
        /// <response code="500">Internal Error</response> 
        [HttpPost("subtract")]
        [MapToApiVersion("1.0")]
        [Produces("application/json")]
        public async Task<ActionResult> SubtractAsync([FromBody] Substract sub)
        {
            try
            {
                if ((sub.MainOperations == null || sub.MainOperations.Count < 2) || (sub.SubOperations == null || sub.SubOperations.Count < 2))
                {
                    Log.Information("Method: " + MethodBase.GetCurrentMethod().DeclaringType.FullName
                                                + "At least two numeric in Minuend and Subtraends are required. " 
                                                + "Numbers of Subtraends: " + (sub.SubOperations == null ? "is null" : sub.SubOperations.Count.ToString()) + " "
                                                + "Numbers of Minuend: " + (sub.MainOperations == null ? "is null" : sub.MainOperations.Count.ToString()));

                    return BadRequest(_errors.GetErrorMessage(400));
                }

                sub.Result = await _calculator.GetSubstractResult(sub);

                if (Request.Headers["X-Evi-Tracking-Id"].Count > 0)
                {
                    await _calculator.SaveTrackingId(Request.Headers["X-Evi-Tracking-Id"], sub, sub);
                    Request.Headers.Remove("X-Evi-Tracking-Id");
                }
                    
                return Ok( new { Difference = sub.Result });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Method: " + MethodBase.GetCurrentMethod().DeclaringType.FullName + " Error: " + ex.Message);
                return StatusCode(500, _errors.GetErrorMessage(500));
            }
        }

        /// <summary>
        /// Returns the multiply of multiple numbers
        /// </summary>
        /// <param name="operands">A list of numbers</param>
        /// <param name="X-Evi-Tracking-Id">optional: id tracking for save the current operand in the database</param>      
        /// <response code="200">Return the result of multiply</response>
        /// <response code="400">At least two numeric operands are required.</response>
        /// <response code="500">Internal Error</response> 
        [HttpPost("multiply")]
        [MapToApiVersion("1.0")]
        [Produces("application/json")]
        public async Task<ActionResult> MultiplyAsync([FromBody] Multiply multi)
        {
            try
            {
                if (multi.MainOperations == null || multi.MainOperations.Count < 2)
                {
                    Log.Information("Method: " + MethodBase.GetCurrentMethod().DeclaringType.FullName + "At least two numeric operands are required.");
                    return BadRequest(_errors.GetErrorMessage(400));
                }

                multi.Result = await _calculator.GetMultiplyResult(multi);
                if (Request.Headers["X-Evi-Tracking-Id"].Count > 0)
                {
                    await _calculator.SaveTrackingId(Request.Headers["X-Evi-Tracking-Id"], multi, null);
                    Request.Headers.Remove("X-Evi-Tracking-Id");
                }
                    

                return Ok(new {Product = multi.Result });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Method: " + MethodBase.GetCurrentMethod() + " Error: " + ex.Message);
                return StatusCode(500, _errors.GetErrorMessage(500));
            }
        }

        /// <summary>
        /// Returns the division of multiple numbers
        /// </summary>
        /// <param name="operands">A list of numbers</param>
        /// <param name="X-Evi-Tracking-Id">optional: id tracking for save the current operand in the database</param>      
        /// <response code="200">Return the result of division</response>
        /// <response code="400">At least two numeric operands are required.</response>
        /// <response code="500">Internal Error</response> 
        [HttpPost("divide")]
        [MapToApiVersion("1.0")]
        [Produces("application/json")]
        public async Task<ActionResult> DivisionAsync([FromBody] Division div)
        {
            try
            {
                if ((div.MainOperations == null || div.MainOperations.Count < 2) || (div.SubOperations == null || div.SubOperations.Count < 2))
                {
                    Log.Information("Method: " + MethodBase.GetCurrentMethod().DeclaringType.FullName + "At least two numeric operands are required.");
                    return BadRequest(_errors.GetErrorMessage(400));
                }

                if (div.MainOperations.Contains(0) || div.SubOperations.Contains(0))
                {
                    Log.Information("Method: " + MethodBase.GetCurrentMethod().DeclaringType.FullName + "Division by zero is not allowed.");
                    return BadRequest(_errors.GetErrorMessage(400));
                }

                var division = await _calculator.GetDivideResult(div);

                if (Request.Headers["X-Evi-Tracking-Id"].Count > 0)
                {
                    await _calculator.SaveTrackingId(Request.Headers["X-Evi-Tracking-Id"], div, div);
                    Request.Headers.Remove("X-Evi-Tracking-Id");
                }
                    

                return Ok(new { division.Quotient , division.Remainder});

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Method: " + MethodBase.GetCurrentMethod().DeclaringType.FullName +" " + ex.Message);
                return StatusCode(500, _errors.GetErrorMessage(500));
            }
        }

        /// <summary>
        /// Returns the square root of a number
        /// </summary>
        /// <param name="operand">A number</param>
        /// <param name="X-Evi-Tracking-Id">optional: id tracking for save the current operand in the database</param>      
        /// <response code="200">Return the result of Square root</response>
        /// <response code="400">At least two numeric operands are required.</response>
        /// <response code="500">Internal Error</response> 
        [HttpPost("sqrt")]
        [MapToApiVersion("1.0")]
        [Produces("application/json")]
        public async Task<ActionResult> SquareRootAsync([FromBody] SquareRoot sqrt)
        {
            try
            {
                if (sqrt.MainOperations == null || sqrt.MainOperations.Count < 1)
                {
                    Log.Information("Method: " + MethodBase.GetCurrentMethod().DeclaringType.FullName + "At least two numeric operands are required.");
                    return BadRequest(_errors.GetErrorMessage(400));
                }

                sqrt.Result = await _calculator.GetSquareResult(sqrt);

                if (Request.Headers["X-Evi-Tracking-Id"].Count > 0)
                {
                    await _calculator.SaveTrackingId(Request.Headers["X-Evi-Tracking-Id"], sqrt, null);
                    Request.Headers.Remove("X-Evi-Tracking-Id");
                }
                    
                return Ok(new { Square = sqrt.Result });

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Method: " + MethodBase.GetCurrentMethod().DeclaringType.FullName + " Error: " + ex.Message);
                return StatusCode(500, _errors.GetErrorMessage(500));
            } 
        }

        /// <summary>
        /// Returns a object with the operations history
        /// </summary>
        /// <param name="trackingId">A string trackingId</param>  
        /// <response code="200">Return the result of division</response>
        /// <response code="400">Tracking ID is required for querying the journal.</response>
        /// <response code="404">No journal entries found for the provided Tracking ID.</response>
        /// <response code="500">Internal Error</response> 
        [HttpGet("journal/{trackingId}")]
        [MapToApiVersion("1.0")]
        [Produces("application/json")]
        public async Task<ActionResult> GetJournalEntriesAsync(string trackingId)
        {
            try
            {
                if (string.IsNullOrEmpty(trackingId))
                {
                    Log.Information("Method: " + MethodBase.GetCurrentMethod().DeclaringType.FullName + "Tracking ID is required for querying the journal." + "Number of trackingId: " + trackingId);
                    return BadRequest("Tracking ID is required for querying the journal.");
                }
                var operations = await _calculator.GetAllTrackingsId(trackingId);
                if (operations.Count == 0)
                {
                    Log.Information("Method: " + MethodBase.GetCurrentMethod().DeclaringType.FullName + "No journal entries found for the provided Tracking ID." + "Number of trackingId: " + trackingId);
                    return NotFound("No journal entries found for the provided Tracking ID.");
                }
                return Ok(new {Operations = operations });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Method: " + MethodBase.GetCurrentMethod().DeclaringType.FullName + " Error: " + ex.Message);
                return StatusCode(500, _errors.GetErrorMessage(500));
            }
           
        }

    }
}
