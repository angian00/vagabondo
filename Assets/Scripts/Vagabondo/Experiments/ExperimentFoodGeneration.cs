﻿using System.Text;
using TMPro;
using UnityEngine;
using Vagabondo.Generators;

namespace Vagabondo.Experiment
{
    public class ExperimentFoodGeneration : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI outputField;



        public void OnRun()
        {
            var foodItem = GameItemGenerator.GenerateIngredientsAndItem(20);

            var textBuilder = new StringBuilder();
            if (foodItem == null)
                textBuilder.Append("No compatible recipe found");
            else
            {
                textBuilder.Append($"name: {foodItem.name} \n");
                textBuilder.Append($"category: {foodItem.subcategory} \n");
                //textBuilder.Append($"preparation: {DataUtils.EnumToStr(foodItem.preparation)} \n");
                textBuilder.Append($"baseValue: {foodItem.baseValue} \n");
                textBuilder.Append($"ingredientCategories: \n");

                //foreach (var ingredient in foodItem.ingredients)
                //    textBuilder.Append($"\t {ingredient.definition.name} \n");
            }

            var generatedText = textBuilder.ToString();
            outputField.text = generatedText;
            Debug.Log("-- generatedText:");
            Debug.Log(generatedText);
        }

    }
}
