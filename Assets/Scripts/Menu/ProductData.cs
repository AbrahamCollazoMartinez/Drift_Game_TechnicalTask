using UnityEngine;
using TMPro;
using System;
using UnityEngine.Purchasing;

public class ProductData : MonoBehaviour
{
	[SerializeField] private TMP_Text priceText;
	[SerializeField] private CodelessIAPButton iapButton;


	public static Action<ProductCatalogItem> setDataProducts = delegate { };

	private void SetDataProduct(ProductCatalogItem dataItem)
	{
		if (iapButton.productId == dataItem.id)
		{
			priceText.text = $"$ {dataItem.pricingTemplateID}";
		}
	}

	private void OnEnable()
	{
		setDataProducts += SetDataProduct;
	}

	private void OnDisable()
	{
		setDataProducts -= SetDataProduct;
	}

}
