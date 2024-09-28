namespace Atata;

/// <summary>
/// An identifier generator that generates a string of 4 alphanumeric characters (A-Z, a-z, 0-9).
/// </summary>
public sealed class Alphanumeric4AtataIdGenerator : IAtataIdGenerator
{
    private readonly char[] _chars = [
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
    ];

    private readonly HashSet<string> _occupiedIdSet = [];

    private readonly object _occupiedIdSetLock = new();

    private readonly Random _random;

    public Alphanumeric4AtataIdGenerator()
        : this(new Random())
    {
    }

    public Alphanumeric4AtataIdGenerator(Random random) =>
        _random = random.CheckNotNull(nameof(random));

    /// <inheritdoc/>
    public string GenerateId()
    {
        string id;
        bool isUniqueId;
        do
        {
            char[] idChars = [
                _chars[_random.Next(0, _chars.Length)],
                _chars[_random.Next(0, _chars.Length)],
                _chars[_random.Next(0, _chars.Length)],
                _chars[_random.Next(0, _chars.Length)]
            ];

            id = new string(idChars);

            lock (_occupiedIdSetLock)
            {
                isUniqueId = _occupiedIdSet.Add(id);
            }
        }
        while (!isUniqueId);

        return id;
    }
}
