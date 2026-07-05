namespace VideoToGifSkill
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Factory to retrieve appropriate IVideoConverter based on file extension.
    /// </summary>
    public class ConverterFactory
    {
        private readonly IReadOnlyDictionary<string, IVideoConverter> _converters;

        public ConverterFactory(IEnumerable<IVideoConverter> converters)
        {
            if (converters == null) throw new ArgumentNullException(nameof(converters));
            // Build a dictionary keyed by lower-cased supported extension for quick lookup
            _converters = converters
                .Where(c => !string.IsNullOrWhiteSpace(c.SupportedExtension))
                .GroupBy(c => c.SupportedExtension.Trim().ToLowerInvariant())
                .ToDictionary(g => g.Key, g => g.First());
        }

        /// <summary>
        /// Returns a converter that can handle the given input file.
        /// Throws InvalidOperationException if no suitable converter is found.
        /// </summary>
        public IVideoConverter GetConverter(string inputPath)
        {
            if (string.IsNullOrWhiteSpace(inputPath))
                throw new ArgumentException("Input path cannot be null or empty.", nameof(inputPath));

            var ext = Path.GetExtension(inputPath).ToLowerInvariant();
            if (_converters.TryGetValue(ext, out var converter))
                return converter;

            // Fallback: if there is only one converter, return it; otherwise error.
            if (_converters.Count == 1)
                return _converters.Values.First();

            throw new InvalidOperationException($"No video converter registered for extension '{ext}'.");
        }
    }
}
