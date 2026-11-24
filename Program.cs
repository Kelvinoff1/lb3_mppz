using System.Text;
using StoreUseCases.Data;
using StoreUseCases.Services;
using StoreUseCases.UI;
using StoreUseCases.UseCases;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

var repository = StoreRepository.CreateWithSeedData();
var info = new StoreInfoService(repository);
var useCases = new UseCaseCatalog(repository);

if (args.Contains("demo"))
    DemoScenario.Run(repository, info, useCases);
else
    new ConsoleMenu(repository, info, useCases).Run();
