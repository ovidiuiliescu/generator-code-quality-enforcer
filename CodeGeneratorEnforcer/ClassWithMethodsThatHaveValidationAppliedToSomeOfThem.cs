using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGeneratorEnforcer
{   
    public class ClassWithMethodsThatHaveValidationAppliedToSomeOfThem
    {
        /// <summary>
        /// This method is enforced via the <see cref="SimpleAttribute"/>
        /// </summary>
        [Simple]
        public void ThisIsOK()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Printing {i}.");
            }
        }

        /// <summary>
        /// This method is also enforced, but should pass validation
        /// </summary>
        [Simple]
        public void ThisIsOKToo()
        {
            Console.WriteLine("No problems here");
        }

        /// <summary>
        /// This method is enforced, but validation should not pass
        /// </summary>
        [Simple]
        public void ThisIsNotOK()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Printing {i}.");
            }

            for (int j = 0; j < 10; j++)
            {
                Console.WriteLine($"Printing {j}.");
            }
        }

        /// <summary>
        /// This method is enforced, but validation should not pass
        /// </summary>
        [Simple]
        public void ThisIsNotOKEither()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Console.WriteLine($"Printing {j}.");
                }
            }
        }

        /// <summary>
        /// This method has no code enforcement
        /// </summary>
        public void ThisIsNotCheckedAtAll()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (i * j * 2 == 3)
                    {
                        Console.WriteLine($"Printing {i} and {j}");
                    }
                }
            }
        }
    }
}
