using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpecSet
{
    public void SetSpec(MeshFilter meshFilter, Renderer renderer, PlatformSpecification specification)
    {
        switch (specification)
        {
            case PlatformSpecification.Selection:
                //TODO
                break;
            case PlatformSpecification.Gold: GoldMeshChanger(meshFilter, renderer); break;
            case PlatformSpecification.Heal: HealMeshChanger(meshFilter, renderer); break;
            case PlatformSpecification.Trap_1: TrapMeshChanger(meshFilter, renderer, PlatformSpecification.Trap_1); break;
            case PlatformSpecification.Trap_2: TrapMeshChanger(meshFilter, renderer, PlatformSpecification.Trap_2); break;
            case PlatformSpecification.Trap_3: TrapMeshChanger(meshFilter, renderer, PlatformSpecification.Trap_3); break;
            case PlatformSpecification.Gift: GiftMeshChanger(meshFilter, renderer); break;
            case PlatformSpecification.Jackpot: JackpotMeshChanger(meshFilter, renderer); break;
            default:
                break;
        }
    }
    private void GoldMeshChanger(MeshFilter filter, Renderer renderer)
    {
        int i = Random.Range(0, 3);
        switch (i)
        {
            case 0:
                filter.mesh = GameManager.GoldMeshes.goldMesh1;
                break;
            case 1:
                filter.mesh = GameManager.GoldMeshes.goldMesh2;
                break;
            case 2:
                filter.mesh = GameManager.GoldMeshes.goldMesh3;
                break;
            default:
                break;
        }
        SetRendererMaterial(renderer);
    }
    private void HealMeshChanger(MeshFilter filter, Renderer renderer)
    {
        int i = Random.Range(0, 2);
        switch (i)
        {
            case 0:
                filter.mesh = GameManager.HealMeshes.healMesh1;
                break;
            case 1:
                filter.mesh = GameManager.HealMeshes.healMesh2;
                break;
            default:
                break;
        }
        SetRendererMaterial(renderer);
    }
    private void TrapMeshChanger(MeshFilter filter, Renderer renderer, PlatformSpecification spec)
    {
        switch (spec)
        {
            case PlatformSpecification.Trap_1:
                filter.mesh = GameManager.TrapMeshes.trapMesh1;
                break;
            case PlatformSpecification.Trap_2:
                filter.mesh = GameManager.TrapMeshes.trapMesh2;
                break;
            case PlatformSpecification.Trap_3:
                filter.mesh = GameManager.TrapMeshes.trapMesh3;
                break;
            default:
                break;
        }
        SetRendererMaterial(renderer);
    }
    private void GiftMeshChanger(MeshFilter filter, Renderer renderer)
    {
        int i = Random.Range(0, 3);
        switch (i)
        {
            case 0:
                filter.mesh = GameManager.RandomBoxMeshes.randomBoxMesh1;
                break;
            case 1:
                filter.mesh = GameManager.RandomBoxMeshes.randomBoxMesh2;
                break;
            case 2:
                filter.mesh = GameManager.RandomBoxMeshes.randomBoxMesh3;
                break;
            default:
                break;
        }
        SetRendererMaterial(renderer);
    }

    private void JackpotMeshChanger(MeshFilter filter, Renderer renderer)
    {
        filter.mesh = GameManager.JackpotMeshes.jackpotMesh;
        SetRendererMaterial(renderer);
    }
    private void SetRendererMaterial(Renderer renderer)
    {
        for (int i = 0; i < renderer.sharedMaterials.Length; i++)
        {
            renderer.sharedMaterials[i].mainTexture = GameManager.PlatformTexture.texture;
        }
    }
}