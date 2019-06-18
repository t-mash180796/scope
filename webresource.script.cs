using Microsoft.SCOPE.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ScopeRuntime;



/// <summary>
/// 
/// </summary>
public class UrlSegregator : Processor {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="columns"></param>
    /// <param name="args"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    public override Schema Produces(string[] columns, string[] args, Schema input) {
        Schema output = new Schema();
        output.Add(new ColumnInfo("YPID", typeof(string)));
        output.Add(new ColumnInfo("url", typeof(string)));
        output.Add(new ColumnInfo("applies_to", typeof(string)));
        output.Add(new ColumnInfo("url_type", typeof(string)));
        return output;
    }
    /// <summary>
    ///
    /// </summary>
    /// <param name="input"></param>
    /// <param name="output"></param>
    /// <param name="args"></param>
    /// <returns></returns>    
    public override IEnumerable<Row> Process(RowSet input, Row output, string[] args) {

        char[] seperator = new[] { '|' };
        foreach (Row row in input.Rows) {
            var urls = (List<string>)(row["URLSubEntity"].Value);
            var applies_tos = (List<string>)(row["AppliesToSubEntity"].Value);
            var url_types = (List<string>)(row["UrlTypeSubEntity"].Value);
            

            int len = url_types.Count;
            output["YPID"].Set(row["YPID"].String);
            for (int i = 0; i < len; ++i) {
                output["url"].Set(urls[i]);
                output["applies_to"].Set(applies_tos[i]);
                output["url_type"].Set(url_types[i]);
                yield return output;
            }

        }
    }
}

/// <summary>
/// 
/// </summary>
public class MyOutputter : Outputter {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <param name="writer"></param>
    /// <param name="args"></param>
    public override void Output(RowSet input, StreamWriter writer, string[] args) {

        BinaryWriter bw = new BinaryWriter(writer.BaseStream);
        foreach (Row row in input.Rows) {
            //bw.Write(row[0].String.Length);
            //bw.Write(System.Text.Encoding.UTF8.GetBytes(row[0].String));
            bw.Flush();  // Flush() is required to mark row boundaries.
        }
    }
}
