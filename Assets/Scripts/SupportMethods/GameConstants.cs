using System.Collections.Generic;

/*
------------------------------------------------------
LOGIC SUMMARY FOR CREDIT VALUE LIST:
------------------------------------------------------
Since to generate an optimal credit value list, we need to find values where: Credit Value × 5 = Min Bet to Max Bet

Since our min bet is 0.10 and max bet is 25.00, we can derive the credit values:

Credit Value = Bet / 5

------------------------------------------------------
Thus, Credit Value List starts as : 

    0.10 / 5 = 0.02,
    0.20 / 5 = 0.04, and so om till, 25.00 / 5 = 5.00
------------------------------------------------------?
*/
public sealed class GameConstants
{
    public static List<double> creditValue = new List<double>
    {
        0.02,
        0.04,
        0.10,
        0.20,
        0.50,
        1.00,
        2.00,
        5.00
    };
}