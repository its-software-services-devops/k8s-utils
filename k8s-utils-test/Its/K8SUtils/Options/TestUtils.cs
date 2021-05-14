using CommandLine;

namespace Its.K8SUtils.Options.Test
{
    public class TestUtils
    {
        public static T TestParseArguments<T>(string[] args)
        {
            T opt = default(T);

            var result = Parser.Default.ParseArguments<T>(args)
                .WithParsed<T>(o => {
                    opt = o;
                }); 

            return opt;
        }           
    }
}