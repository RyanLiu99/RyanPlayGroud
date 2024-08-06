


namespace webapi
{
    public class TestCounter
    {
        //The System.Diagnostics.Metrics.Meter type is the entry point for a library to create a named group of instruments.
        static System.Diagnostics.Metrics.Meter s_meter = new Meter("HatCo.Store");
        static System.Diagnostics.Metrics.Counter<int> s_hatsSold = s_meter.CreateCounter<int>("hatco.store.hats_sold");

        new OpenTelemetry.Metrics.MeterProvider
    }
}
