using System;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Moon : MonoBehaviour
{
    private Renderer _renderer;
    private Material _material;

    // Mean synodic month (days)
    private const double SynodicMonthDays = 29.530588853;

    // Reference FULL moon in UTC (from your list)
    // Feb 1, 2026 @ 22:09 UTC
    private static readonly DateTime FullMoonEpochUtc =
        new DateTime(2026, 2, 1, 22, 9, 0, DateTimeKind.Utc);

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _material = _renderer.material; // creates an instance material
    }

    private void Start()
    {
        float phase = GetPhase01(DateTime.UtcNow);
        _material.SetFloat("_Phase", phase);
        // Debug.Log($"Moon _Phase = {phase:F6}");
    }

    // 0..1 where:
    // 0/1 = new moon, 0.5 = full moon
    private float GetPhase01(DateTime utcNow)
    {
        // Days since the reference full moon
        double days = (utcNow - FullMoonEpochUtc).TotalDays;

        // Fractional cycles in [0,1)
        double frac = (days / SynodicMonthDays) % 1.0;
        if (frac < 0.0) frac += 1.0;

        // Shift so "full" occurs at 0.5
        float p = Mathf.Repeat((float)(frac + 0.5), 1f);

        // Auto-reverse (fixes flipped visuals)
        p = 1f - p;

        // Keep in [0,1)
        return Mathf.Repeat(p, 1f);
    }
}
