﻿#nullable enable

namespace Atata;

internal sealed class DecomposedUri : ICloneable
{
    public DecomposedUri()
    {
    }

    public DecomposedUri(string uri) =>
        Parse(uri);

    public string FullPath { get; set; } = string.Empty;

    public string Query { get; set; } = string.Empty;

    public string Fragment { get; set; } = string.Empty;

    [return: NotNullIfNotNull(nameof(value))]
    public static implicit operator string?(DecomposedUri? value) =>
        value?.ToString();

    private static (string Fragment, int IndexOfHash) ExtractFragment(string uri)
    {
        int indexOfHash = uri.IndexOf('#');

        return indexOfHash >= 0
            ? (uri[(indexOfHash + 1)..], indexOfHash)
            : (string.Empty, -1);
    }

    private void Parse(string uri)
    {
        if (!string.IsNullOrWhiteSpace(uri))
        {
            string workingUri = uri;

            if (workingUri[0] is '&' or ';')
                workingUri = $"?{workingUri[1..]}";

            var (fragment, indexOfHash) = ExtractFragment(workingUri);

            if (indexOfHash >= 0)
            {
                Fragment = fragment;
                workingUri = workingUri[..indexOfHash];
            }

            int indexOfQuestionMark = workingUri.IndexOf('?');

            if (indexOfQuestionMark < 0)
            {
                FullPath = workingUri;
            }
            else
            {
                FullPath = uri[..indexOfQuestionMark];
                Query = workingUri[(indexOfQuestionMark + 1)..];
            }
        }
    }

    public static DecomposedUri Merge(string uri1, string uri2) =>
        new DecomposedUri(uri1).MergeWith(uri2);

    public DecomposedUri AppendQuery(string query)
    {
        if (!string.IsNullOrEmpty(query))
        {
            string queryWithoutPrefix = query[0] is '?' or '&' or ';'
                ? query[1..]
                : query;

            Query = string.IsNullOrEmpty(Query)
                ? queryWithoutPrefix
                : $"{Query}{(query[0] == ';' ? ';' : '&')}{queryWithoutPrefix}";
        }

        return this;
    }

    public DecomposedUri MergeWith(string uri)
    {
        if (uri[0] == '#')
        {
            Fragment = uri[1..];
        }
        else if (uri[0] is '&' or ';')
        {
            var (fragment, indexOfHash) = ExtractFragment(uri);
            Fragment = fragment;

            AppendQuery(indexOfHash < 0 ? uri : uri[..indexOfHash]);
        }
        else if (uri[0] == '?')
        {
            var (fragment, indexOfHash) = ExtractFragment(uri);
            Fragment = fragment;

            Query = indexOfHash < 0 ? uri[1..] : uri[1..indexOfHash];
        }
        else
        {
            FullPath = string.Empty;
            Query = string.Empty;
            Fragment = string.Empty;

            Parse(uri);
        }

        return this;
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder(FullPath);

        if (!string.IsNullOrEmpty(Query))
            builder.Append('?').Append(Query);

        if (!string.IsNullOrEmpty(Fragment))
            builder.Append('#').Append(Fragment);

        return builder.ToString();
    }

    object ICloneable.Clone() =>
        Clone();

    public DecomposedUri Clone() =>
        (DecomposedUri)MemberwiseClone();
}
