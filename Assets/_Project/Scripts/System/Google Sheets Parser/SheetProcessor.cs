using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SheetProcessor
{
    private enum RowTypes
	{
        Id=0,
        Hp,
        Damage,
        MoveSpeed,
        AttackRadius,
        ChaseRadius,
	}

    private const char _cellSeporator = ',';
    private const char _inCellSeporator = ';';

    private Dictionary<string, Color> _colors = new Dictionary<string, Color>()
    {
        {"white", Color.white},
        {"black", Color.black},
        {"yellow", Color.yellow},
        {"red", Color.red},
        {"green", Color.green},
        {"blue", Color.blue},
    };
    
    public EntityData ProcessData(string cvsRawData)
    {
        char lineEnding = GetPlatformSpecificLineEnd();
        string[] rows = cvsRawData.Split(lineEnding);
        int dataStartRawIndex = 1;
        EntityData data = new EntityData();
        for (int i = dataStartRawIndex; i < rows.Length; i++)
        {
            string[] cells = rows[i].Split(_cellSeporator);
            var id = cells[(int)RowTypes.Id];
            var hp = ParseFloat(cells[(int)RowTypes.Hp]);
            var damage = ParseFloat(cells[(int)RowTypes.Damage]);
            var speed = ParseFloat(cells[(int)RowTypes.MoveSpeed]);
            var attackRadius = ParseFloat(cells[(int)RowTypes.AttackRadius]);
            var chaseRadius = ParseFloat(cells[(int)RowTypes.ChaseRadius]);

            var entitySpecs = ScriptableObject.CreateInstance<EntitySpecs>();
            entitySpecs.Id = id;
            entitySpecs.Hp = hp;
            entitySpecs.Damage = damage;
            entitySpecs.MoveSpeed = speed;
            entitySpecs.AttackRadius = attackRadius;
            entitySpecs.ChaseRadius = chaseRadius;

            data.EntitiesOptions.Add(entitySpecs);
        }
        return data;
    }

    private Color ParseColor(string color)
    {
        color = color.Trim();
        Color result = default;
        if (_colors.ContainsKey(color))
        {
            result = _colors[color];
        }
        
        return result;
    }

    private Vector3 ParseVector3(string s)
    {
        string[] vectorComponents = s.Split(_inCellSeporator);
        if (vectorComponents.Length < 3)
        {
            Debug.LogError("Can't parse Vector3. Wrong text format");
            return default;
        }

        float x = ParseFloat(vectorComponents[0]);
        float y = ParseFloat(vectorComponents[1]);
        float z = ParseFloat(vectorComponents[2]);
        return new Vector3(x, y, z);
    }
    
    private int ParseInt(string s)
    {
        int result = -1;
        if (!int.TryParse(s, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.GetCultureInfo("en-US"), out result))
        {
            Debug.LogError("Can't parse int, wrong text");
        }

        return result;
    }
    
    private float ParseFloat(string s)
    {
        float result = -1;
        if (!float.TryParse(s, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.GetCultureInfo("en-US"), out result))
        {
            Debug.LogError("Can't pars float,wrong text ");
        }

        return result;
    }
    
    private char GetPlatformSpecificLineEnd()
    {
        char lineEnding = '\n';
#if UNITY_IOS
        lineEnding = '\r';
#endif
        return lineEnding;
    }
}