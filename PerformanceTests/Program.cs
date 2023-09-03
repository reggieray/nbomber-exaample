using NBomber.CSharp;

internal class Program
{
    private static void Main(string[] args)
    {
        using var httpClient = new HttpClient();

        var scenario = Scenario.Create("load_test", async context =>
            {
                var response = await httpClient.GetAsync("http://localhost:5195/weatherforecast");

                return response.IsSuccessStatusCode
                    ? Response.Ok()
                    : Response.Fail();
            })
            .WithoutWarmUp()
            .WithLoadSimulations(
                Simulation.Inject(rate: 30, 
                                  interval: TimeSpan.FromSeconds(1),
                                  during: TimeSpan.FromSeconds(30))
            );

            NBomberRunner
                .RegisterScenarios(scenario)
                .Run();
    }
}