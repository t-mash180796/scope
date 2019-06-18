using Microsoft.SCOPE.Types;
using System;

using System.Collections.Generic;
using System.IO;
using System.Text;
using ScopeRuntime;

/// <summary>
/// 
/// </summary>


public class TextSeperator : Processor {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="columns"></param>
    /// <param name="args"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    public override Schema Produces(string[] columns, string[] args, Schema input) {
        return input;
    }
    /// <summary>
    ///
    /// </summary>
    /// <param name="input"></param>
    /// <param name="output"></param>
    /// <param name="args"></param>
    /// <returns></returns>    
    public override IEnumerable<Row> Process(RowSet input, Row output, string[] args) {
        var splitchars = new[] { '|' };
        foreach (Row row in input.Rows) {
            var id_list = row["text"].String.Split(splitchars);
            row.CopyTo(output);
            foreach (var id in id_list) {
                output["text"].Set(id);
                yield return output;
            }
        }
    }
}