using System;
using System.Collections.Generic;
using System.IO;

namespace AmazingCo.DataLayer
{
    public class InFileCompanyStructureDataLayer : ICompanyStructureDataLayer
    {
        // I assumed the company chart is not going to change that often.
        // Based on my researches, to improve the performance of the updates,
        // instead of file system, I could use an in memory key/value stores
        // that have builtin persistence capabilities (for example Redis).

        public InFileCompanyStructureDataLayer()
        {
            const string fileName = "CompanyStructureGraph.txt";

            _separators = new []{","};

            _file = new FileInfo(fileName);

            if (!_file.Exists)
                throw new FileNotFoundException(_file.FullName);
        }

        private readonly string[] _separators;
        private readonly FileInfo _file;

        public IEnumerable<NodeSnapshot> GetAll()
        {
            var line = 0;
            using (var reader = _file.OpenText())
            {
                while (!reader.EndOfStream)
                {
                    line++;
                    NodeSnapshot snapshot;
                    try
                    {
                        var parts = reader
                            .ReadLine()?
                            .Split(
                                _separators,
                                StringSplitOptions.RemoveEmptyEntries
                            );

                        if (parts?.Length >= 0)
                            snapshot = new NodeSnapshot(
                                int.Parse(parts[0]),
                                parts[1],
                                int.Parse(parts[2])
                            );
                        else
                            throw new Exception("Three columns in each row expected.");

                    }
                    catch (Exception ex)
                    {
                        throw new FileLoadException($"Invalid format in line {line}.", ex);
                    }

                    yield return snapshot;
                }
            }
        }

        public void WriteAll(IEnumerable<NodeSnapshot> nodes)
        {
            try
            {
                var tempFile = new FileInfo(
                    Path.Combine(
                        _file.DirectoryName ?? "",
                        $"temp_{_file?.Name}"
                    )
                );
                using (var writer = tempFile.CreateText())
                {
                    var separator = 
                        _separators.Length > 0 
                            ? _separators[0] 
                            : ",";

                    foreach (var nodeSnapshot in nodes)
                        writer.WriteLine(
                            $"{nodeSnapshot.Id}{separator}{nodeSnapshot.Title}{separator}{nodeSnapshot.ParentId}"
                        );

                    writer.Flush();
                }

                _file.Delete();

                tempFile.MoveTo(_file.FullName);
            }
            catch
            {
                throw;
            }
        }
    }
}
