using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public List<(Vector3, Template)> templates = new List<(Vector3, Template)>();
    public int levelValue = 0;
    public Level(List<(Vector3, Template template)> templates)
    {
        this.templates = templates;

        foreach ((Vector3, Template template) tuple in templates)
        {
            this.levelValue += tuple.template.value;
        }
    }

    public Level() { }

    public void AddTemplate(Vector3 position, Template template)
    {
        this.templates.Add((position, template));
        this.levelValue += template.value;
    }
}
