namespace CodeGeneratorEnforcer
{
    internal class Program
    {
        // Not really needed, but what the heck.
        static void Main(string[] args)
        {
            var instance = new ClassWithMethodsThatHaveValidationAppliedToSomeOfThem();
            instance.ThisIsOK();
            instance.ThisIsOKToo();
            instance.ThisIsNotOK();
            instance.ThisIsNotOKEither();
            instance.ThisIsNotCheckedAtAll();
        }
    }
}
