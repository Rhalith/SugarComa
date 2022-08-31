using Assets.MainBoard.Scripts.Utils.PlatformUtils;
using Assets.MainBoard.Scripts.GameManaging;
using UnityEngine;

namespace Assets.MainBoard.Scripts.Route
{
    public class PlatformSpecSet
    {
        public void SetSpec(MeshFilter meshFilter, Renderer renderer, PlatformSpec specification)
        {
            switch (specification)
            {
                case PlatformSpec.Selection:
                    //TODO
                    break;
                case PlatformSpec.Gold: GoldMeshChanger(meshFilter, renderer); break;
                case PlatformSpec.Heal: HealMeshChanger(meshFilter, renderer); break;
                case PlatformSpec.Trap: TrapMeshChanger(meshFilter, renderer); break;
                case PlatformSpec.Gift: GiftMeshChanger(meshFilter, renderer); break;
                case PlatformSpec.Jackpot: JackpotMeshChanger(meshFilter, renderer); break;
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
        private void TrapMeshChanger(MeshFilter filter, Renderer renderer)
        {
            int i = Random.Range(0, 3);
            switch (i)
            {
                case 0:
                    filter.mesh = GameManager.TrapMeshes.trapMesh1;
                    break;
                case 1:
                    filter.mesh = GameManager.TrapMeshes.trapMesh2;
                    break;
                case 2:
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
}