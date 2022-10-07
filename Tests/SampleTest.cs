namespace Tests;

public class SampleTest {
    private static class Calculator {
        public static int Add(int x, int y) => x + y;
        public static int Subtract(int x, int y) => x - y;
    }
    
    [Fact]
    public void Test1() {
        Assert.Equal(2, Calculator.Add(1, 1));
    }
    
    [Fact]
    public void Test2() {
        Assert.Equal(2, Calculator.Subtract(1, 1));
    }
}