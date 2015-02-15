﻿using System;
using System.Linq;
using Newtonsoft.Json;
using Riskified.SDK.Model.OrderElements;
using Riskified.SDK.Utils;

namespace Riskified.SDK.Model
{
    
    public class Order : AbstractOrder
    {
        /// <summary>
        /// Creates a new order
        /// </summary>
        /// <param name="merchantOrderId">The unique id of the order at the merchant systems</param>
        /// <param name="email">The email used for contact in the order</param>
        /// <param name="customer">The customer information</param>
        /// <param name="paymentDetails">The payment details</param>
        /// <param name="billingAddress">Billing address</param>
        /// <param name="shippingAddress">Shipping address</param>
        /// <param name="lineItems">An array of all products in the order</param>
        /// <param name="shippingLines">An array of all shipping details for the order</param>
        /// <param name="gateway">The payment gateway that was used</param>
        /// <param name="customerBrowserIp">The customer browser ip that was used for the order</param>
        /// <param name="currency">A three letter code (ISO 4217) for the currency used for the payment</param>
        /// <param name="totalPrice">The sum of all the prices of all the items in the order, taxes and discounts included</param>
        /// <param name="createdAt">The date and time when the order was created</param>
        /// <param name="updatedAt">The date and time when the order was last modified</param>
        /// <param name="discountCodes">An array of objects, each one containing information about an item in the order (optional)</param>
        /// <param name="totalDiscounts">The total amount of the discounts on the Order (optional)</param>
        /// <param name="cartToken">Unique identifier for a particular cart or session that is attached to a particular order. The same ID should be passed in the Beacon JS (optional)</param>
        /// <param name="totalPriceUsd">The price in USD (optional)</param>
        /// <param name="closedAt">The date and time when the order was closed. If the order was closed (optional)</param>
        /// <param name="financialStatus">The financial status of the order (could be paid/voided/refunded/partly_paid/etc.)</param>
        /// <param name="fulfillmentStatus">The fulfillment status of the order</param>
        /// <param name="source">The source of the order</param>
        /// <param name="noChargeDetails">No charge sums - including all payments made for this order in giftcards, cash, checks or other non chargebackable payment methods</param>
        public Order(int merchantOrderId, string email, Customer customer,
            AddressInformation billingAddress, AddressInformation shippingAddress, LineItem[] lineItems,
            ShippingLine[] shippingLines,
            string gateway, string customerBrowserIp, string currency, double totalPrice, DateTime createdAt,
            DateTime updatedAt,
            IPaymentDetails paymentDetails = null, DiscountCode[] discountCodes = null, double? totalDiscounts = null, string cartToken = null, double? totalPriceUsd = null, 
            DateTime? closedAt = null,string financialStatus = null,string fulfillmentStatus = null,string source = null, NoChargeDetails noChargeDetails = null) : base(merchantOrderId)
        {
            LineItems = lineItems;
            ShippingLines = shippingLines;
            BillingAddress = billingAddress;
            ShippingAddress = shippingAddress;
            Customer = customer;
            Email = email;
            CustomerBrowserIp = customerBrowserIp;
            Currency = currency;
            TotalPrice = totalPrice;
            Gateway = gateway;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            
            // optional fields
            PaymentDetails = paymentDetails;
            DiscountCodes = discountCodes;
            TotalPriceUsd = totalPriceUsd;
            TotalDiscounts = totalDiscounts;
            CartToken = cartToken;
            ClosedAt = closedAt;
            FinancialStatus = financialStatus;
            FulfillmentStatus = fulfillmentStatus;
            Source = source;
            NoChargeAmount = noChargeDetails;
        }

        /// <summary>
        /// Validates the objects fields content
        /// </summary>
        /// <param name="isWeak">Should use weak validations or strong</param>
        /// <exception cref="OrderFieldBadFormatException">throws an exception if one of the parameters doesn't match the expected format</exception>
        public override void Validate(bool isWeak = false)
        {
            base.Validate(isWeak);
            InputValidators.ValidateObjectNotNull(LineItems, "Line Items");
            LineItems.ToList().ForEach(item => item.Validate(isWeak));
            InputValidators.ValidateObjectNotNull(ShippingLines, "Shipping Lines");
            ShippingLines.ToList().ForEach(item => item.Validate(isWeak));
            if(PaymentDetails == null && NoChargeAmount == null)
            {
                throw new Exceptions.OrderFieldBadFormatException("Both PaymentDetails and NoChargeDetails are missing - at least one should be specified");
            }
            if(PaymentDetails != null)
            {
                PaymentDetails.Validate(isWeak);
            }
            else 
            {
                NoChargeAmount.Validate(isWeak);
            }

            if (isWeak)
            {
                if (BillingAddress == null && ShippingAddress == null)
                {
                    throw new Exceptions.OrderFieldBadFormatException("Both shipping and billing addresses are missing - at least one should be specified");
                }

                if (BillingAddress != null)
                {
                    BillingAddress.Validate(isWeak);
                }
                else
                {
                    ShippingAddress.Validate(isWeak);
                }
            }
            else
            {
                InputValidators.ValidateObjectNotNull(BillingAddress, "Billing Address");
                BillingAddress.Validate(isWeak);
                InputValidators.ValidateObjectNotNull(ShippingAddress, "Shipping Address");
                ShippingAddress.Validate(isWeak);
            }

            InputValidators.ValidateObjectNotNull(Customer, "Customer");
            Customer.Validate(isWeak);
            InputValidators.ValidateEmail(Email);
            InputValidators.ValidateIp(CustomerBrowserIp);
            InputValidators.ValidateCurrency(Currency);
            InputValidators.ValidateZeroOrPositiveValue(TotalPrice.Value, "Total Price");
            InputValidators.ValidateValuedString(Gateway, "Gateway");
            InputValidators.ValidateDateNotDefault(CreatedAt.Value, "Created At");
            InputValidators.ValidateDateNotDefault(UpdatedAt.Value, "Updated At");
            
            // optional fields validations
            if(DiscountCodes != null && DiscountCodes.Length > 0)
            {
                DiscountCodes.ToList().ForEach(item => item.Validate(isWeak));
            }
            if(TotalPriceUsd.HasValue)
            {
                InputValidators.ValidateZeroOrPositiveValue(TotalPriceUsd.Value, "Total Price USD");
            }
            if (TotalDiscounts.HasValue)
            {
                InputValidators.ValidateZeroOrPositiveValue(TotalDiscounts.Value, "Total Discounts");
            }
            if (ClosedAt.HasValue)
            {
                InputValidators.ValidateDateNotDefault(ClosedAt.Value, "Closed At");
            }
        }

        
        /// <summary>
        /// Overrides order object fields with an order checkout object fields (non null fields).
        /// </summary>
        /// <param name="orderCheckout">an order checkout object that his fields will be assign to the current order fields.</param>
        public void ImportOrderCheckout(OrderCheckout orderCheckout)
        {
            if(!string.IsNullOrEmpty(orderCheckout.CartToken))
            {
                this.CartToken = orderCheckout.CartToken;
            }

            if (orderCheckout.LineItems != null)
            {
                this.LineItems = orderCheckout.LineItems;
            }

            if (orderCheckout.ShippingAddress != null)
            {
                this.ShippingAddress = orderCheckout.ShippingAddress;
            }

            if (orderCheckout.PaymentDetails != null)
            {
                this.PaymentDetails = orderCheckout.PaymentDetails;
            }

            if (orderCheckout.NoChargeAmount != null)
            {
                this.NoChargeAmount = orderCheckout.NoChargeAmount;
            }

            if (orderCheckout.BillingAddress != null)
            {
                this.BillingAddress = orderCheckout.BillingAddress;
            }

            if (orderCheckout.ShippingAddress != null)
            {
                this.ShippingAddress = orderCheckout.ShippingAddress;
            }

            if (orderCheckout.Customer != null)
            {
                this.Customer = orderCheckout.Customer;
            }
            if (!string.IsNullOrEmpty(orderCheckout.Email))
            {
                this.Email = orderCheckout.Email;
            }
            if (!string.IsNullOrEmpty(orderCheckout.CustomerBrowserIp))
            {
                this.CustomerBrowserIp = orderCheckout.CustomerBrowserIp;
            }

            if (!string.IsNullOrEmpty(orderCheckout.Currency))
            {
                this.Currency = orderCheckout.Currency;
            }

            if (!string.IsNullOrEmpty(orderCheckout.Gateway))
            {
                this.Gateway = orderCheckout.Gateway;
            }

            if (!string.IsNullOrEmpty(orderCheckout.FinancialStatus))
            {
                this.FinancialStatus = orderCheckout.FinancialStatus;
            }
            if (orderCheckout.TotalPrice.HasValue)
            {
                this.TotalPrice = orderCheckout.TotalPrice;
            }
            if(orderCheckout.CreatedAt != null)
            {
                this.CreatedAt = orderCheckout.CreatedAt;
            }
            if(orderCheckout.UpdatedAt != null)
            {
                this.UpdatedAt = orderCheckout.UpdatedAt;
            }

            if (orderCheckout.DiscountCodes != null)
            {
                this.DiscountCodes = orderCheckout.DiscountCodes;
            }

            if (orderCheckout.TotalPriceUsd.HasValue)
            {
                this.TotalPriceUsd = orderCheckout.TotalPriceUsd;
            }

            if (orderCheckout.TotalDiscounts.HasValue)
            {
                this.TotalDiscounts = orderCheckout.TotalDiscounts;
            }

            if (orderCheckout.ClosedAt.HasValue)
            {
                this.ClosedAt = orderCheckout.ClosedAt;
            }

            
        }
        
        
        /// <summary>
        /// The session id that this order was created on, this value should match the session id value that is passed in the beacon JavaScript.
        /// </summary>
        [JsonProperty(PropertyName = "cart_token")]
        public string CartToken { get; set; }

        /// <summary>
        /// The date and time when the order was closed.
        /// </summary>
        [JsonProperty(PropertyName = "closed_at")]
        public DateTime? ClosedAt { get; set; }

        /// <summary>
        /// The date and time when the order was first created.
        /// </summary>
        [JsonProperty(PropertyName = "created_at")]
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The three letter code (ISO 4217) for the currency used for the payment.
        /// </summary>
        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }

        /// <summary>
        /// The customer's email address.
        /// </summary>
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        /// <summary>
        /// The payment gateway used.
        /// </summary>
        [JsonProperty(PropertyName = "gateway")]
        public string Gateway { get; set; }

        /// <summary>
        /// The total amount of the discounts to be applied to the price of the order.
        /// </summary>
        [JsonProperty(PropertyName = "total_discounts")]
        public double? TotalDiscounts { get; set; }

        /// <summary>
        /// The sum of all the prices of all the items in the order, taxes and discounts included (must be positive).
        /// </summary>
        [JsonProperty(PropertyName = "total_price")]
        public double? TotalPrice { get; set; }

        [JsonProperty(PropertyName = "total_price_usd")]
        public double? TotalPriceUsd { get; set; }

        /// <summary>
        /// The date and time when the order was last modified.
        /// </summary>
        [JsonProperty(PropertyName = "updated_at")]
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// The IP address of the browser used by the customer when placing the order.
        /// </summary>
        [JsonProperty(PropertyName = "browser_ip")]
        public string CustomerBrowserIp { get; set; }
        
        /// <summary>
        /// A list of discount code objects, each one containing information about an item in the order.
        /// </summary>
        [JsonProperty(PropertyName = "discount_codes")]
        public DiscountCode[] DiscountCodes { get; set; }

        /// <summary>
        /// A list of line item objects, each one containing information about an item in the order.
        /// </summary>
        [JsonProperty(PropertyName = "line_items")]
        public LineItem[] LineItems { get; set; }

        /// <summary>
        /// A list of shipping line objects, each of which details the shipping methods used.
        /// </summary>
        [JsonProperty(PropertyName = "shipping_lines")]
        public ShippingLine[] ShippingLines { get; set; }

        /// <summary>
        /// An object containing information about the payment.
        /// </summary>
        [JsonProperty(PropertyName = "payment_details")]
        public IPaymentDetails PaymentDetails { get; set; }

        /// <summary>
        /// The mailing address associated with the payment method.
        /// </summary>
        [JsonProperty(PropertyName = "billing_address")]
        public AddressInformation BillingAddress { get; set; }

        /// <summary>
        /// The mailing address to where the order will be shipped.
        /// </summary>
        [JsonProperty(PropertyName = "shipping_address")]
        public AddressInformation ShippingAddress { get; set; }

        /// <summary>
        /// An object containing information about the customer.
        /// </summary>
        [JsonProperty(PropertyName = "customer")]
        public Customer Customer { get; set; }

        [JsonProperty(PropertyName = "financial_status")]
        public string FinancialStatus { get; set; }

        [JsonProperty(PropertyName = "fulfillment_status")]
        public string FulfillmentStatus { get; set; }

        [JsonProperty(PropertyName = "source")]
        public string Source { get; set; }

        [JsonProperty(PropertyName = "nocharge_amount")]
        public NoChargeDetails NoChargeAmount { get; set; }

        [JsonProperty(PropertyName = "vendor_id")]
        public string VendorId { get; set; }

        [JsonProperty(PropertyName = "vendor_name")]
        public string VendorName { get; set; }
    }

}
