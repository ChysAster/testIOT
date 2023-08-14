using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MCT.Functions;
using testIOT.models;

namespace MCT.Function
{
  public static class CalculatorTrigger
  {
    [FunctionName("CalculatorTrigger")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "calculator/{getal1}/{getal2}/{op}")] HttpRequest req, int getal1, int getal2, string op,
        ILogger log)
    {


      try
      {
        int som = 0;
        switch (op)
        {
          case "+":
            som = getal1 + getal2;
            break;
          case "-":
            som = getal1 - getal2;
            break;
          case "*":
            som = getal1 * getal2;
            break;
          default:
            som = 000;
            break;
        }

        CalculationResult calculation = new CalculationResult
        {
          Operator = op,
          Result = som.ToString()
        };

        return new OkObjectResult(calculation);
      }
      catch (System.Exception)
      {

        throw;
      }
    }
  }

  public static class CalculatorRequest
  {
    [FunctionName("CalculatorRequest")]
    public static async Task<IActionResult> Start(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "calculator")] HttpRequest req,
            ILogger log)
    {


      try
      {
        string body = await new StreamReader(req.Body).ReadToEndAsync();
        CalculationRequest data = JsonConvert.DeserializeObject<CalculationRequest>(body);
        string result = String.Empty;
        if (data.Operator == "+")
        {
          result = (data.getal1 + data.getal2).ToString();
        }
        else if (data.Operator == "-")
        {
          result = (data.getal1 - data.getal2).ToString();
        }
        else if (data.Operator == "*")
        {
          result = (data.getal1 * data.getal2).ToString();
        }
        else if (data.Operator == "/")
        {
          result = (data.getal1 / data.getal2).ToString();
        }
        else
        {
          result = "Operator not supported";
        }
        CalculationResult calculationResult = new CalculationResult
        {
          Operator = data.Operator,
          Result = result
        };
        return new OkObjectResult(calculationResult);
      }
      catch (System.Exception)
      {

        throw;
      }
    }
  }
}
