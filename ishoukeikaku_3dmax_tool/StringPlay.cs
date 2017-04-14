using System;
using System.Collections.Generic;
using System.IO;

class StringPlay
{
    // check that the first n chars are an int
    public static bool FirstCharsAreInt (string test_string, int num_of_chars) {
        bool valid = true;

        for (int i=0; i<num_of_chars; i++) {
            char h = test_string[i];
            if (!System.Char.IsDigit(h)) {
                valid = false;
            };
        };

        return valid;
    }

    // wraps string in '
    public static string SQuoteWrap (string test_string) {
        return "'" + test_string + "'";
    }

    // creates a safe sql string (enclosed by ' sign)
    public static string SQLSafeString (string test_string) {
        string test_string2 = test_string.Replace("'", "''");
        if (test_string.Length > 2) {
            char last_char = test_string2[test_string2.Length - 1];
            if (last_char == Path.DirectorySeparatorChar) {
                return SQuoteWrap(test_string2.Remove(test_string.Length - 1));
            } else {
                return SQuoteWrap(test_string2);
            };
        } else {
            return SQuoteWrap(test_string2);
        };
    }
}

