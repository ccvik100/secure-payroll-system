# Secure Payroll System 💼🔒

Egy modern, mikroszolgáltatás-alapú bérszámfejtő és dolgozói nyilvántartó rendszer. A projekt ipari sztenderdeknek megfelelő DevOps és GitOps folyamatokat használ, felhő-natív (Cloud-Native) környezetre felkészítve.

## 🏗 Architektúra és Komponensek

A rendszer egymástól független, Docker konténerekben futó mikroszolgáltatásokból áll, amelyeket a Kubernetes vezérel.

* **Frontend (`/frontend`):** Angular alapú UI, Material Design formavilággal.
* **Directory Service (`/backend/DirectoryService`):** .NET 10 C# API az alapvető dolgozói adatokhoz.
* **Vault Service (`/backend/VaultService`):** .NET 10 C# API a szenzitív pénzügyi adatokhoz.
* **Finance MCP (`/backend/FinanceMcp`):** .NET 10 C# API AI/MCP integrációhoz.
* **Adatbázis:** MongoDB konténer a perzisztens adattároláshoz.

## 🚀 Technológiai Stack

* **Frontend:** Angular 17+, TypeScript, Angular Material
* **Backend:** C#, .NET 10 (ASP.NET Core)
* **Konténerizáció:** Docker, GHCR (GitHub Container Registry)
* **Orkesztráció:** Kubernetes, ArgoCD (GitOps)

## 🔄 CI/CD Folyamat

A projekt **GitHub Actions** pipeline-t használ a folyamatos integrációhoz (CI). Minden push után automatikusan épülnek a Docker image-ek, amiket az **ArgoCD** (CD) szinkronizál a lokális Kubernetes klaszterrel a `k8s/` mappában található leírók alapján.

## 🛠 Futtatás Kubernetes Környezetben (Lokális)

A rendszer futtatásához Docker Desktop (bekapcsolt Kubernetes-szel) szükséges.

### 1. ArgoCD telepítése
```bash
# Namespace létrehozása
kubectl create namespace argocd

# ArgoCD telepítése (Szerveroldali alkalmazás a méretkorlátok miatt)
kubectl apply -n argocd --server-side --force-conflicts -f [https://raw.githubusercontent.com/argoproj/argo-cd/stable/manifests/install.yaml](https://raw.githubusercontent.com/argoproj/argo-cd/stable/manifests/install.yaml)

# Jelszó lekérése (felhasználónév: admin)
kubectl -n argocd get secret argocd-initial-admin-secret -o jsonpath="{.data.password}" | base64 -d; echo
```

### 2. Alkalmazás indítása
1. Nyisd meg az ArgoCD-t: `kubectl port-forward svc/argocd-server -n argocd 8080:443`
2. Hozz létre egy új alkalmazást (**New App**):
   * **Name:** `secure-payroll`
   * **Repository:** (Ide másold a saját GitHub repód linkjét)
   * **Path:** `k8s`
   * **Destination:** `https://kubernetes.default.svc` (Namespace: `default`)
3. Kattints a **Sync** gombra.

### 3. Elérés (Localhost)
Mivel a szolgáltatások `LoadBalancer` típusúak, a Docker Desktop kivezeti őket a gazdagépre:
* **Webes felület:** [http://localhost:4200](http://localhost:4200)
* **Directory API:** [http://localhost:5001](http://localhost:5001)
* **Vault API:** [http://localhost:5002](http://localhost:5002)
* **Finance MCP:** [http://localhost:5003](http://localhost:5003)

## 📂 Mappaszerkezet

```text
/backend               - Mikroszolgáltatások forráskódja
/frontend              - Angular UI forráskódja
/k8s                   - Kubernetes YAML leírók (Deployment, Service)
/.github/workflows     - CI pipeline konfiguráció
```