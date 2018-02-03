using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Currency {
	Gold,
	Electrocity,
}

[AssetPath("items")]
public class Item : IDatabaseRow {
	public int id { get; private set; }
	public string name { get; private set; }
	public string description { get; private set; }
	public int price { get; private set; }
	public Currency currency { get; private set; }
	public bool installable { get; private set; } // 설치형인가? false라면 소모형
	public string[] showConditions { get; private set; } // 상점 출현 조건

    int IDatabaseRow.ID {
        get { return this.id; }
    }

    bool IDatabaseRow.Parse(List<string> row)
    {
		Debug.Assert(row[0].StartsWith("I"));
		if (!row[0].StartsWith("I"))
			return false;

		id = int.Parse(row[0].Substring(1));
		name = row[2];
		description = row[4];

		var pricing = row[5].Split(':');
		price = int.Parse(pricing[1]);
		currency = pricing[0] == "E" ? Currency.Electrocity : Currency.Gold;

		installable = row[6] == "설치형";
		showConditions = new string[2]{row[8], row[9]};

		return true;
    }
}
