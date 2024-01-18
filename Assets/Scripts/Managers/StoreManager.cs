using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Redcode.Extensions;

public class StoreManager : MonoBehaviour, IStoreListener
{
	[SerializeField] private GameObject unlockCarMessage;
	[SerializeField] private TMP_Text driftcoinsdisplay;
	[SerializeField] private bool UseFakeStore = false;

	private Action OnPurchaseCompleted;
	private IStoreController StoreController;
	private IExtensionProvider ExtensionProvider;

	private int _driftcoinsPlayer = 0;
	public int driftcoinsPlayer
	{
		set
		{
			_driftcoinsPlayer = value;
			UpdateDriftcoins();
		}
		get { return _driftcoinsPlayer; }
	}

	public static StoreManager instance;

	private async void Awake()
	{
		unlockCarMessage.SetActive(false);

		instance = this;
		driftcoinsPlayer = SaveSystem.Instance.ReadStoreData();

		InitializationOptions options = new InitializationOptions()
#if UNITY_EDITOR || DEVELOPMENT_BUILD
			.SetEnvironmentName("test");
#else
			.SetEnvironmentName("production");
#endif
		await UnityServices.InitializeAsync(options);
		ResourceRequest operation = Resources.LoadAsync<TextAsset>("IAPProductCatalog");
		operation.completed += HandleIAPCatalogLoaded;
	}
	#region IapCoinsSection
	private void HandleIAPCatalogLoaded(AsyncOperation Operation)
	{
		ResourceRequest request = Operation as ResourceRequest;

		Debug.Log($"Loaded Asset: {request.asset}");
		ProductCatalog catalog = JsonUtility.FromJson<ProductCatalog>((request.asset as TextAsset).text);
		Debug.Log($"Loaded catalog with {catalog.allProducts.Count} items");

		SendDataToProducts(catalog.allProducts);

		if (UseFakeStore) // Use bool in editor to control fake store behavior.
		{
			StandardPurchasingModule.Instance().useFakeStoreUIMode = FakeStoreUIMode.StandardUser; // Comment out this line if you are building the game for publishing.
			StandardPurchasingModule.Instance().useFakeStoreAlways = true; // Comment out this line if you are building the game for publishing.
		}

#if UNITY_ANDROID
		ConfigurationBuilder builder = ConfigurationBuilder.Instance(
			StandardPurchasingModule.Instance(AppStore.GooglePlay)
		);
#elif UNITY_IOS
		ConfigurationBuilder builder = ConfigurationBuilder.Instance(
			StandardPurchasingModule.Instance(AppStore.AppleAppStore)
		);
#else
		ConfigurationBuilder builder = ConfigurationBuilder.Instance(
			StandardPurchasingModule.Instance(AppStore.NotSpecified)
		);
#endif
		foreach (ProductCatalogItem item in catalog.allProducts)
		{
			builder.AddProduct(item.id, item.type);
		}

		Debug.Log($"Initializing Unity IAP with {builder.products.Count} products");
		UnityPurchasing.Initialize(this, builder);
	}

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		StoreController = controller;
		ExtensionProvider = extensions;
		Debug.Log($"Successfully Initialized Unity IAP. Store Controller has {StoreController.products.all.Length} products");

	}

	private void SendDataToProducts(ICollection<ProductCatalogItem> items)
	{
		foreach (var item in items)
		{
			ProductData.setDataProducts?.Invoke(item);
		}
	}


	private void HandlePurchase(Product Product, Action OnPurchaseCompleted)
	{

		this.OnPurchaseCompleted = OnPurchaseCompleted;
		StoreController.InitiatePurchase(Product);
	}

	public void RestorePurchase() // Use a button to restore purchase only in iOS device.
	{
#if UNITY_IOS
		ExtensionProvider.GetExtension<IAppleExtensions>().RestoreTransactions(OnRestore);
#endif
	}

	public void OnInitializeFailed(InitializationFailureReason error)
	{
		Debug.LogError($"Error initializing IAP because of {error}." +
			$"\r\nShow a message to the player depending on the error.");
	}

	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		Debug.Log($"Failed to purchase {product.definition.id} because {failureReason}");
		OnPurchaseCompleted?.Invoke();
		OnPurchaseCompleted = null;


	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
	{

		GrantCoins(purchaseEvent);
		Debug.Log($"Successfully purchased {purchaseEvent.purchasedProduct.definition.id} ");
		OnPurchaseCompleted?.Invoke();
		OnPurchaseCompleted = null;



		// do something, like give the player their currency, unlock the item,
		// update some metrics or analytics, etc...

		return PurchaseProcessingResult.Complete;
	}

	public void OnInitializeFailed(InitializationFailureReason error, string message)
	{

	}


	private void UpdateDriftcoins()
	{
		driftcoinsdisplay.text = driftcoinsPlayer.ToString();
		SaveSystem.Instance.SaveStoreData(driftcoinsPlayer);
	}

	private void GrantCoins(PurchaseEventArgs itemPurchased)
	{
		switch (itemPurchased.purchasedProduct.definition.id)
		{

			case "500 drifcoins":
				driftcoinsPlayer = driftcoinsPlayer + 500;
				break;

			case "1000 drifcoins":

				driftcoinsPlayer = driftcoinsPlayer + 1000;
				break;
		}
	}
	#endregion

	#region StoreCarsSection

	public void tryToPruchaseCar(int requiredDrifcoins)
	{
		if (driftcoinsPlayer >= requiredDrifcoins)
		{
			//unlock car succesful
			driftcoinsPlayer -= requiredDrifcoins;
			OpenMessageUnlockCar(true);
		}
	}

	public void OpenMessageUnlockCar(bool state)
	{
		if (state)
		{
			unlockCarMessage.SetActive(true);

			unlockCarMessage.transform.localScale = Vector3.zero;
			unlockCarMessage.transform.DOScale(1, 0.5f).SetEase(Ease.InOutQuint);
		}
		else
		{
			unlockCarMessage.transform.DOScale(0, 0.5f).SetEase(Ease.InOutQuint);

		}
	}

	#endregion

}
