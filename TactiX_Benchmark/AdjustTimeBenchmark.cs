using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using TactiX_Models.Tactics;

namespace TactiX_Benchmark;

public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<AdjustTimeBenchmark>();
    }
}

[MemoryDiagnoser]
public class AdjustTimeBenchmark
{
    private List<LReplayAction>? _testData;
    private List<LReplayAction>? _testDataWorstCase;

    [GlobalSetup]
    public void Setup()
    {
        // 创建测试数据
        _testData = CreateTestData(1000);
        _testDataWorstCase = CreateWorstCaseData(1000);
    }

    [Benchmark(Baseline = true)]
    public List<LReplayAction> AdjustTime_NormalCase()
    {
        // 深拷贝避免修改原始数据
        var copy = _testData!.Select(a => new LReplayAction
        {
            UnitName = a.UnitName,
            Abbr = a.Abbr,
            Time = a.Time,
            Number = a.Number,
            Gameloop = a.Gameloop,
            Supply = a.Supply
        }).ToList();

        return TactiX_ModSupport.Sc2ReplayDecoder.AdjustTime(copy);
    }

    [Benchmark]
    public List<LReplayAction> AdjustTime_WorstCase()
    {
        // 最坏情况：所有事件在同一时间
        var copy = _testDataWorstCase!.Select(a => new LReplayAction
        {
            UnitName = a.UnitName,
            Abbr = a.Abbr,
            Time = a.Time,
            Number = a.Number,
            Gameloop = a.Gameloop,
            Supply = a.Supply
        }).ToList();

        return TactiX_ModSupport.Sc2ReplayDecoder.AdjustTime(copy);
    }

    private static List<LReplayAction> CreateTestData(int count)
    {
        var random = new Random(42); // 固定种子以便复现
        var units = new[] { "Marine", "SCV", "Zergling", "Probe", "Zealot" };
        var list = new List<LReplayAction>(count);

        for (int i = 0; i < count; i++)
        {
            list.Add(new LReplayAction
            {
                UnitName = units[random.Next(units.Length)],
                Abbr = "T",
                Time = random.Next(1000),
                Number = 1,
                Gameloop = i * 16,
                Supply = "10/20"
            });
        }

        return list;
    }

    private static List<LReplayAction> CreateWorstCaseData(int count)
    {
        // 最坏情况：所有事件在同一时间，不同单位
        var units = new[] { "Marine", "SCV", "Zergling", "Probe", "Zealot", "Stalker", "Immortal" };
        var list = new List<LReplayAction>(count);

        for (int i = 0; i < count; i++)
        {
            list.Add(new LReplayAction
            {
                UnitName = units[i % units.Length] + i, // 确保单位名不同
                Abbr = "T",
                Time = 100, // 所有事件同一时间
                Number = 1,
                Gameloop = i * 16,
                Supply = "10/20"
            });
        }

        return list;
    }
}
