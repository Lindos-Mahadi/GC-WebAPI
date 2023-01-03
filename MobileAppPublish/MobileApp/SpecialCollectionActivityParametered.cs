﻿using System;
using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using PMS.Droid.Classes.OffLineHelpers;
using PMS.Droid.Helpers;

namespace PMS.Droid
{
    [Activity(Label = "Special Collection", Theme = "@style/Theme.DesignDemo", WindowSoftInputMode = SoftInput.AdjustPan, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SpecialCollectionActivityParametered : BaseActivity
    {
        private long SelectedMemberID = 0;
        private int MemberSelectedIndex = 0;

        private long KParameteredMemberID = 0;
        private string KParameteredMemberName = "";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var KActivityKey = Intent.Extras.GetLong("MemberId");
            var KActivityMemberName = Intent.Extras.GetString("MemberName");

            KParameteredMemberID = KActivityKey;
            KParameteredMemberName = KActivityMemberName;



            //var centerInfo = SessionHelper.CenterList.Where(w => w.CenterName == text).FirstOrDefault();



            SetContentView(Resource.Layout.SpecialCollection);
            GenerateNavigationMenu();
            try
            {
                SetSpinners();

                var syncUserHelper = new UserOfflineHelper(this);
                var user = syncUserHelper.GetUser();
                if (user != null && !string.IsNullOrEmpty(user.InstallmentDate))
                {
                    // var lblSamityAndDate = FindViewById<TextView>(Resource.Id.lblDate);
                    DateTime dt;
                    if (DateTime.TryParse(user.InstallmentDate, out dt))
                        Title = "Sp. Collection - " + dt.ToString("MMM dd, yyyy");

                }

                var btnSave = FindViewById<Button>(Resource.Id.btnSaveCollection);
                btnSave.Click += delegate
                {
                    CommonHelper.Busy(this);
                    try
                    {
                        var isValid = true;
                        var spnMember = FindViewById<Spinner>(Resource.Id.spnMemberCollection);

                        //var txtMemberCode = FindViewById<EditText>(Resource.Id.txtMemberCode);
                        if (spnMember.SelectedItem == null)
                        {
                            GeatMessage("No Member selected.", false);
                            CommonHelper.Done(this);
                            return;
                        }
                        var memberCode = spnMember.SelectedItem.ToString();
                        var lv = FindViewById<ListView>(Resource.Id.loanProductCollectionList);
                        var adapter = lv.Adapter as LoanCollectionProductGridAdapter;
                        var productData = adapter.GetAllData();
                        if (isValid)
                        {
                            var spnCenter = FindViewById<Spinner>(Resource.Id.spnCenterCollection);
                            // var spnProduct = FindViewById<Spinner>(Resource.Id.spnProductCollection);

                            var officeID = SessionHelper.OfficeID;
                            if (spnCenter.SelectedItem == null)
                            {
                                GeatMessage("No Samity selected.", false);
                                CommonHelper.Done(this);
                                return;
                            }
                            var centerId = GetCenterId(spnCenter.SelectedItem.ToString());
                            var memberID = GetMemberId(spnMember.SelectedItem.ToString());
                            // var productId = 0;// GetProductId(spnProduct.SelectedItem.ToString(), memberID);
                            var syncHelper = new LoanCollectionOfflineHelper(this);
                            foreach (var item in productData)
                            {
                                syncHelper.AddNewCollection(new LoanCollectionModel()
                                {
                                    OfficeID = SessionHelper.OfficeID,
                                    OfficeName = SessionHelper.OfficeName,
                                    CenterID = centerId,
                                    CenterName = spnCenter.SelectedItem.ToString(),
                                    ProductID = item.ProductID,
                                    ProductName = item.ProductName,// spnProduct.SelectedItem.ToString(),                           
                                    MemberCode = memberCode,
                                    MemberID = memberID,
                                    Amount = item.TodayCollectionAmount,
                                    DueAmount = item.Recoverable,
                                    TrxType = 2,
                                    ProductType = item.ProductType,
                                    SyncFlag = 0,
                                    SummaryID = item.SummaryID,
                                    IntCharge = item.IntCharge,
                                    LoanInstallment = item.LoanInstallment,
                                    IntInstallment = item.IntInstallment
                                });
                            }
                            // txtAmount.Text = "";
                            SetMemberSpinner(centerId);
                            GeatMessage("Special Loan Collection created successfully", true);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogSystemError(UIItems.SpecialCollection + " - Save", ex.Message, "");
                        GeatMessage("Error occured." + ex.Message, false);
                    }
                    CommonHelper.Done(this);
                };
                var btnNext = FindViewById<Button>(Resource.Id.btnNext);
                btnNext.Click += delegate
                {
                    var spnMember = FindViewById<Spinner>(Resource.Id.spnMemberCollection);
                    var totalCount = spnMember.Adapter.Count;
                    if (MemberSelectedIndex < totalCount - 1)
                        MemberSelectedIndex = MemberSelectedIndex + 1;
                    else
                        MemberSelectedIndex = 0;
                    spnMember.SetSelection(MemberSelectedIndex);
                };
                var btnPrevious = FindViewById<Button>(Resource.Id.btnPrevious);
                btnPrevious.Click += delegate
                {
                    var spnMember = FindViewById<Spinner>(Resource.Id.spnMemberCollection);
                    var totalCount = spnMember.Adapter.Count;
                    if (MemberSelectedIndex <= 0)
                        MemberSelectedIndex = 0;
                    else
                        MemberSelectedIndex = MemberSelectedIndex - 1;
                    spnMember.SetSelection(MemberSelectedIndex);
                };
            }
            catch (Exception ex)
            {
                LogSystemError(UIItems.SpecialCollection + " - Onload", ex.Message, "");
            }
        }
        private void SetSpinners()
        {
            CommonHelper.Busy(this);
            SetCenterSpinners();
            var spnCenter = FindViewById<Spinner>(Resource.Id.spnCenterCollection);
            spnCenter.ItemSelected += SpnCenter_ItemSelected;
            var spnMember = FindViewById<Spinner>(Resource.Id.spnMemberCollection);
            spnMember.ItemSelected += SpnMember_ItemSelected;
            CommonHelper.Done(this);
        }

        private void SpnProduct_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var obj = ((Spinner)sender).SelectedItem.ToString();
            if (!string.IsNullOrEmpty(obj))
            {
                LoadProductDetails(obj);
            }
        }
        private void LoadProductDetails(string product)
        {
            var mbrProductHelper = new MemberProductOfflineHelper(this);
            var productDetails = mbrProductHelper.GetProductByMemberAndName(product, SelectedMemberID);
            // var txtSelectedProductDetails = FindViewById<EditText>(Resource.Id.txtSelectedProductDetails);
            var detailTxt = "";
            switch (productDetails.ProductType)
            {
                case 0:
                    detailTxt = string.Format("Deposit: {0}, Recoverable: {1}, Balance: {2}, Ins: {3}", productDetails.LoanRecovery, productDetails.Recoverable, productDetails.Balance, productDetails.InstallmentNo);
                    break;
                case 1:
                    detailTxt = string.Format("Recovery: {0}, Recoverable: {1}, Balance: {2}, Ins: {3}", productDetails.LoanRecovery, productDetails.Recoverable, productDetails.Balance, productDetails.InstallmentNo);
                    break;
                default:
                    break;
            }
            // txtSelectedProductDetails.Text = detailTxt;
        }
        private void SetCenterSpinners()
        {
            var officeId = SessionHelper.OfficeID;
            if (default(int) != officeId)
            {
                var syncHelperMem = new MemberOfflineHelper(this);
                MemberModel mem = syncHelperMem.GetById(KParameteredMemberID);

                var syncHelper = new CenterOfflineHelper(this);

                //var lst = syncHelper.GetCentersByOffice(officeId, 2);
                var lst = syncHelper.GetCentersByOffice(mem.OfficeID, 2);
                lst = lst.Where(x=> x.CenterID == mem.CenterID).ToList();


                SessionHelper.CenterList = lst;
                var spnCenter = FindViewById<Spinner>(Resource.Id.spnCenterCollection);
                var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, lst.Select(s => s.CenterName).ToList());
                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                spnCenter.Adapter = adapter;
            }
            else
                GeatMessage("No office selected. Please select ofice and then try again.", false);
        }


        private void SpnCenter_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var obj = ((Spinner)sender).SelectedItem.ToString();
            if (!string.IsNullOrEmpty(obj))
            {
                var centerId = GetCenterId(obj);
                MemberSelectedIndex = 0;
                SetMemberSpinner(centerId);
            }
        }

        private void SetMemberSpinner(int centerId)
        {
            var syncHelper = new MemberOfflineHelper(this);
            //var lst = syncHelper.GetMembersByCenterForCollection(centerId, 2);

            List<MemberModel> lst2 = new List<MemberModel>();

            MemberModel mem = syncHelper.GetById(KParameteredMemberID);
            var lst = syncHelper.GetMembersByCenterForCollection(mem.CenterID, 2);
            lst = lst.Where(x => x.MemberID == KParameteredMemberID).ToList();
            //lst.Clear();
           // lst.Add();

            //lst.Add(lst);

            var spnMember = FindViewById<Spinner>(Resource.Id.spnMemberCollection);
            var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, lst.Select(s => s.MemberName).ToList());  //.Where(x => x.MemberID == KParameteredMemberID)
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spnMember.Adapter = adapter;
            if (MemberSelectedIndex > 0 && lst.Count > 0)
            {
                if (MemberSelectedIndex == lst.Count)
                    MemberSelectedIndex = 0;
                else
                    MemberSelectedIndex = lst.Count - 1 > MemberSelectedIndex ? MemberSelectedIndex : lst.Count - 1;
                spnMember.SetSelection(MemberSelectedIndex);
            }
            if (lst.Count == 0)
                LoadGrid(null);
        }
        private void SpnMember_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {

                MemberSelectedIndex = 1; //e.Position;
                var obj = ((Spinner)sender).SelectedItem.ToString();
                if (KParameteredMemberName != "")
                {
                    obj = KParameteredMemberName.ToString();
                }
                LoadGrid(obj);
            }
            catch (Exception ex)
            {
                LogSystemError(UIItems.SpecialCollection + " -SpnMember_ItemSelected", ex.Message, "");
                GeatMessage("Error occured. " + ex.Message, false);
            }
        }
        private void LoadGrid(string memberCode)
        {
            var listItsms = new List<MemberProductModel>();
            var todalCollectionTextBox = FindViewById<TextView>(Resource.Id.txtTodayCollection);
            if (!string.IsNullOrEmpty(memberCode))
            {
                var memberID = GetMemberId(memberCode);
                SelectedMemberID = memberID;
                var dbVals = new MemberProductOfflineHelper(this);
                listItsms = dbVals.GetMemberProducts(memberID, 2);
            }
            var lv = FindViewById<ListView>(Resource.Id.loanProductCollectionList);
            lv.Adapter = new LoanCollectionProductGridAdapter(this, listItsms, todalCollectionTextBox, lv);
            // lv.ItemClick += Lv_ItemClick;
            var sum = listItsms.Sum(s => s.LoanRecovery);
            var totalRecovery = FindViewById<TextView>(Resource.Id.txtTotalRecovery); //txtTotalRecovery 
            totalRecovery.Text = sum.ToString();
            todalCollectionTextBox.Text = sum.ToString();
        }
        private void Lv_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var pos = e.Position;
            var view = e.View;
            var amt = view.FindViewById<EditText>(Resource.Id.txtLoanProductAmountCollection);
            var txt = amt.Text;
        }

        private long GetMemberId(string text)
        {
            var syncHelper = new MemberOfflineHelper(this);
            var member = syncHelper.GetByName(text);
            if (member != null)
                return member.MemberID;
            return 0;
        }

        //private void SetProductSpinners(long memberID)
        //{
        //    var syncHelper = new MemberProductOfflineHelper(this);
        //    var lst = syncHelper.GetProductsByMember(memberID);
        //    var spn = FindViewById<Spinner>(Resource.Id.spnProductCollection);
        //    var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, lst.Select(s => s.ProductName).ToList());
        //    adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
        //    spn.Adapter = adapter;
        //}
        private bool RequiredFieldValidation(EditText control, string message)
        {
            var isValid = true;
            if (string.IsNullOrWhiteSpace(control.Text.Trim()))
            {
                new CustomMessage(this, this, message, false).Show();
                isValid = false;
                control.FindFocus();
            }
            return isValid;
        }
        private bool ValidateAmount(EditText control, string text)
        {
            var isValid = true;
            var amt = 0.0d;
            if (!double.TryParse(control.Text, out amt))
            {
                new CustomMessage(this, this, text, false).Show();
                isValid = false;
                control.FindFocus();
            }
            return isValid;
        }

        private string GetMemberCode(string text)
        {
            var splits = text.Split(",".ToArray());
            if (splits.Length > 1)
                return splits[0];
            return "";
        }
        private int GetProductId(string text, long memberID)
        {
            var syncHelper = new MemberProductOfflineHelper(this);
            var item = syncHelper.GetProductByMemberAndName(text, memberID);
            var result = 0;
            if (item != null)
                result = item.ProductID;
            return result;
        }
        private int GetPurposeId(string text)
        {
            var result = 0;
            var item = SessionHelper.PurposeList.Where(w => w.PurposeName == text).FirstOrDefault();
            if (item != null)
                result = item.PurposeID;
            return result;
        }
        private int GetCenterId(string text)
        {
            var result = 0;
            var item = SessionHelper.CenterList.Where(w => w.CenterName == text).FirstOrDefault();
            if (item != null)
                result = item.CenterID;
            return result;
        }
        private void GeatMessage(string message, bool flag)
        {
            new CustomMessage(this, this, message, flag).Show();
        }
        private void LogException(Exception ex)
        {
            new CustomMessage(this, this, ex).Show();
        }
    }
}
