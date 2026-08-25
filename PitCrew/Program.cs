//Read input; set variables

var input = File.ReadAllText("input1.txt");
var lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
var firstValue = 0;
var increases = 0;

//Go through the input line by line; check for increases.
foreach(var line in lines)
{
    if (int.TryParse(line, out var value))
    {
        if (value > firstValue && firstValue != 0)
            {
                increases++;
            };
        firstValue = value;
    }
}
Console.WriteLine(increases + " increases for Day 1");

///////////////////////////////////////////////////////////////////

//Read input for Day 25
var input25 = File.ReadAllLines("input25.txt").ToList();

//Setup the map
var Map = new char[2, input25.Count, input25[0].Length];

for (int i = 0; i < input25.Count; i++)
    for (int j = 0; j < input25[0].Length; j++)
    {
        Map[0, i, j] = input25[i][j];
    }

long moves = 0;
bool hasMoved = true;

//Run the cucumber simulation until no valid moves are made
while (hasMoved)
{
    hasMoved = false;
    // move east
    for (int i = 0; i < input25.Count; i++)
        for (int j = 0; j < input25[0].Length; j++)
        {
            if (Map[0, i, j] == '>')
            {
                int k = (j == input25[0].Length - 1) ? 0 : j + 1;
                if (Map[0, i, k] == '.')
                {
                    Map[1, i, j] = '.';
                    hasMoved = true;
                }
                else Map[1, i, j] = '>';
            }
            else if (Map[0, i, j] == '.')
            {
                int k = (j == 0) ? input25[0].Length - 1 : j - 1;
                if (Map[0, i, k] == '>')
                {
                    Map[1, i, j] = '>';
                }
                else Map[1, i, j] = '.';
            }
            else Map[1, i, j] = Map[0, i, j];
        }

    for (int i = 0; i < input25.Count; i++)
        for (int j = 0; j < input25[0].Length; j++)
        {
            if (Map[1, i, j] == 'v')
            {
                int k = (i == input25.Count - 1) ? 0 : i + 1;
                if (Map[1, k, j] == '.')
                {
                    Map[0, i, j] = '.';
                    hasMoved = true;
                }
                else Map[0, i, j] = 'v';
            }
            else if (Map[1, i, j] == '.')
            {
                int k = (i == 0) ? input25.Count - 1 : i - 1;
                if (Map[1, k, j] == 'v')
                {
                    Map[0, i, j] = 'v';
                    hasMoved = true;
                }
                else Map[0, i, j] = '.';
            }
            else
                Map[0, i, j] = Map[1, i, j];
        }

    moves++;
}
Console.WriteLine(moves + " moves before the cucumbers stop moving in Day 25");

///////////////////////////////////////////////////////////////////

//Read input for Day 24
var input24 = File.ReadAllLines("input24.txt").ToList();
var evaluate = new List<(string key, long value)>[] { new List<(string key, long value)>() };

string ChangeParameter(string input, int location, int value)
{
    var c = input.ToArray();
    c[location] = (char)(value + '0');
    return new string(c);
}

(int w, int x, int y, int z) SetValue(string variable, int value, (int w, int x, int y, int z) variables)
{
    if (variable == "w") variables.w = value;
    else if (variable == "x") variables.x = value;
    else if (variable == "y") variables.y = value;
    else if (variable == "z") variables.z = value;
    return variables;
}

int GetValue(string variable, (int w, int x, int y, int z) variables)
{
    if (variable == "w") return variables.w;
    if (variable == "x") return variables.x;
    if (variable == "y") return variables.y;
    if (variable == "z") return variables.z;
    return 0;
}

bool IsVariable(string v) { return (v == "w" || v == "x" || v == "y" || v == "z"); }
int[] intArray(string s) { return s.Select(c => (int)(c - '0')).ToArray(); }

((int w, int x, int y, int z) c, int error) RunProgram(string[] program, (int w, int x, int y, int z) variables, string modelnumber)
{
    var input = intArray(modelnumber);
    int i = 0;
    foreach (string s in program)
    {
        var p1 = s.Split(' ');
        if (p1[0] == "inp")
        {
            variables = SetValue(p1[1], input[i], variables);
            i++;
        }
        else
        {
            int a = GetValue(p1[1], variables);
            int b = IsVariable(p1[2]) ? GetValue(p1[2], variables) : int.Parse(p1[2]);

            if (p1[0] == "add")
            {
                variables = SetValue(p1[1], a + b, variables);
            }
            else if (p1[0] == "mul")
            {
                variables = SetValue(p1[1], a * b, variables);
            }
            else if (p1[0] == "div")
            {
                if (b > 0) variables = SetValue(p1[1], a / b, variables);
            }
            else if (p1[0] == "mod")
            {
                if (a > 0 && b >= 0) variables = SetValue(p1[1], a % b, variables);
            }
            else if (p1[0] == "eql")
            {
                variables = SetValue(p1[1], a == b ? 1 : 0, variables);
            }
        }
    }
    return (variables, 0);
}
