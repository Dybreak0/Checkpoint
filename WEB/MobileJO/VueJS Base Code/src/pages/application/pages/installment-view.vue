<template>
  <v-form v-model="valid" ref="form" class="child-body">
    <confirm ref="confirm"></confirm>
    <info ref="info"></info>
    <loading v-if="fullscreenLoading"></loading>
    <offline @detected-condition="handleConnectivityChange"></offline>
    <v-layout row>
      <v-card flat>
        <h2 class="d-flex align-center">
          <v-icon x-large>description</v-icon>Installment Application Details
        </h2>
      </v-card>
    </v-layout>
    <v-layout row>
      <v-divider></v-divider>
    </v-layout>
    <v-spacer class="formsSpacer"></v-spacer>
    <v-layout row>
      <v-flex>
        <v-card class="search-filter-vcard ma-md-3" flat tile>
          <v-layout row wrap>
            <v-flex xs12 md6 px-2>
              <v-layout row wrap align-center mb-3>
                <v-flex xs12 sm4 class="input-label text-xs-left text-sm-right">
                  <span>Application #:</span>
                </v-flex>
                <v-flex xs12 sm8>
                  <h1>{{ model.application_no }}</h1>
                </v-flex>
              </v-layout>
              <v-layout row wrap align-center>
                <v-flex xs12 sm4 class="input-label text-xs-left text-sm-right">
                  <span>Branch: <b>*</b></span>
                </v-flex>
                <v-flex xs12 sm8>
                  <v-select
                    v-model="personalInfo.SelectedBranch"
                    menu-props="auto"
                    label="Please Select"
                    :items="branches"
                    :rules="rPersonalInfo.SelectedBranch"
                    :class="{ 'readonly-style': isEdit }"
                    required
                    :disabled="!isEdit"
                  ></v-select>
                </v-flex>
              </v-layout>
              <v-layout row wrap align-center>
                <v-flex xs12 sm4 class="input-label text-xs-left text-sm-right">
                  <span>Client Name: <b>*</b></span>
                </v-flex>
                <v-flex xs12 sm8>
                  <v-text-field
                    v-model="personalInfo.Name"
                    :rules="rPersonalInfo.Name"
                    required
                    clearable
                    :disabled="!isEdit"
                  ></v-text-field>
                </v-flex>
              </v-layout>
              <v-layout row wrap align-center>
                <v-flex xs12 sm4 class="input-label text-xs-left text-sm-right">
                  <span>Birth Date: <b>*</b> </span>
                </v-flex>
                <v-flex xs12 sm8>
                  <v-menu
                    v-model="chooseBirthDate"
                    transition="scale-transition"
                    :close-on-content-click="false"
                    lazy
                    offset-y
                    full-width
                  >
                    <template v-slot:activator="{ on }">
                      <v-text-field
                        v-model="personalInfo.BirthDate"
                        v-on="on"
                        append-icon="event"
                        readonly
                        placeholder="YYYY-MM-DD"
                        color="red"
                        :rules="rPersonalInfo.BirthDate"
                        required
                        :disabled="!isEdit"
                      >
                      </v-text-field>
                    </template>
                    <v-date-picker
                      class="customTable"
                      color="red"
                      v-model="personalInfo.BirthDate"
                      @change="CalculateAge"
                      @input="chooseBirthDate = false"
                      :max="new Date().toISOString().substr(0, 10)"
                    >
                    </v-date-picker>
                  </v-menu>
                </v-flex>
              </v-layout>
              <v-layout row wrap align-center mt-1>
                <v-flex xs12 sm4 class="input-label text-xs-left text-sm-right">
                  <span>Age: <b>*</b></span>
                </v-flex>
                <v-flex xs12 sm8>
                  <span>{{ personalInfo.Age }}</span>
                </v-flex>
              </v-layout>
              <v-layout row wrap align-center my-4>
                <v-flex xs12 sm4 class="input-label text-xs-left text-sm-right">
                  <span>Application Status:</span>
                </v-flex>
                <v-flex xs12 sm8>
                  <span class="red--text headline font-italic">{{
                    model.loan_status
                  }}</span>
                </v-flex>
              </v-layout>
            </v-flex>
            <v-flex xs12 md6 px-2>
              <v-layout row wrap align-center>
                <v-flex xs5 sm4 class="input-label text-xs-left text-sm-right">
                  <span>Client Signatory: </span>
                </v-flex>
                <v-flex xs12 sm8>
                  <span
                    ><v-icon>attach_file</v-icon
                    ><a
                      @click="
                        download(
                          model.loan_id,
                          model.client_signature,
                          'ClientSignature'
                        )
                      "
                      >{{ model.client_signature }}</a
                    ></span
                  >
                </v-flex>
              </v-layout>
              <v-layout row wrap align-center mt-4>
                <v-flex xs5 sm4 class="input-label text-xs-left text-sm-right">
                  <span>Created: </span>
                </v-flex>
                <v-flex xs12 sm8>
                  <span
                    >{{ model.created_by_name }} ({{
                      model.created_date
                    }})</span
                  >
                </v-flex>
              </v-layout>
              <v-layout row wrap align-center mt-4>
                <v-flex xs5 sm4 class="input-label text-xs-left text-sm-right">
                  <span>Updated: </span>
                </v-flex>
                <v-flex xs12 sm8>
                  <span v-if="!!model.updated_by_name">
                    {{ model.updated_by_name }}
                    ({{ model.updated_date }})
                  </span>
                  <span v-else>-</span>
                </v-flex>
              </v-layout>
              <v-layout row wrap align-center mt-4>
                <v-flex xs5 sm4 class="input-label text-xs-left text-sm-right">
                  <span>Approved: </span>
                </v-flex>
                <v-flex xs12 sm8>
                  <span v-if="!!model.approved_by_name">
                    {{ model.approved_by_name }}
                    ({{ model.approved_date }})
                  </span>
                  <span v-else>-</span>
                </v-flex>
              </v-layout>
            </v-flex>
          </v-layout>
          <v-layout row mt-3>
            <v-flex v-if="isEdit" text-xs-right>
              <v-btn @click="Cancel" class="btn_secondary">
                <v-icon left>keyboard_return</v-icon>
                Cancel
              </v-btn>
              <v-btn @click="Save" class="btn_primary">
                <v-icon left>save</v-icon>
                Save
              </v-btn>
            </v-flex>
            <v-layout v-if="!isEdit" wrap>
              <v-flex
                text-xs-center
                text-md-left
                v-if="model.loan_status == 'Pending'"
              >
                <v-btn
                  @click="UpdateLoanStatus('Approved')"
                  class="btn_primary"
                >
                  <v-icon left>thumb_up</v-icon>&nbsp;Approve
                </v-btn>
                <v-btn
                  @click="UpdateLoanStatus('Denied')"
                  class="btn_black mr-auto"
                >
                  <v-icon left>thumb_down</v-icon>&nbsp;Deny
                </v-btn>
              </v-flex>
              <v-flex text-xs-center text-md-right>
                <v-btn @click="Back" class="btn_secondary">
                  <v-icon left>keyboard_return</v-icon>
                  Back
                </v-btn>
                <v-btn @click="Delete" class="btn_primary">
                  <v-icon left>close</v-icon>
                  Delete
                </v-btn>
                <v-btn
                  @click="Edit"
                  class="btn_primary"
                  v-if="model.loan_status == 'Pending'"
                >
                  <v-icon left>edit</v-icon>
                  Edit
                </v-btn>
              </v-flex>
            </v-layout>
          </v-layout>
        </v-card>
      </v-flex>
    </v-layout>
    <v-layout row wrap justify-start>
      <v-flex xs12 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2"
              >Home Address (Present) <b class="red--text">*</b></b
            >
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="personalInfo.HomeAddress"
              color="purple darken-2"
              label="Click here to get Home Address (Present)"
              single-line
              outline
              clearable
              :rules="rPersonalInfo.HomeAddress"
              readonly
              required
              @click="selectHomeAddress = true"
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-dialog v-model="selectHomeAddress" persistent max-width="600px">
        <v-card>
          <v-card-title class="font-weight-medium grey lighten-2" primary-title>
            Please select your Present Home Address
          </v-card-title>
          <v-card-text>
            <v-layout row wrap>
              <v-flex xs12 sm4 px-2>
                <v-select
                  v-model="personalInfo.Region"
                  @change="RegionChange(personalInfo.Region.value)"
                  :items="regions"
                  return-object
                  menu-props="auto"
                  label="Region/State/Province*"
                  hide-details
                  required
                  :disabled="!isEdit"
                ></v-select>
                <span class="v-input error--text v-messages">
                  <span v-if="hasRegion">
                    Please select a Region
                  </span>
                </span>
              </v-flex>
              <v-flex xs12 sm4 px-2>
                <v-select
                  v-model="personalInfo.City"
                  :items="cities"
                  return-object
                  menu-props="auto"
                  label="City*"
                  :disabled="Object.keys(personalInfo.Region).length === 0"
                  hide-details
                  required
                ></v-select>
                <span class="v-input error--text v-messages">
                  <span v-if="hasCity">
                    Please select a City
                  </span>
                </span>
              </v-flex>
              <v-flex xs12 sm4 px-2>
                <v-text-field
                  v-model="personalInfo.City.code"
                  label="Zip Code"
                  disabled
                  hide-details
                  required
                  style="padding-top: 21px;"
                ></v-text-field>
              </v-flex>
              <v-flex xs12 px-2>
                <v-text-field
                  v-model="personalInfo.HouseUnitBuildingNo"
                  label="House/Unit/Building no.*"
                  hide-details
                  required
                ></v-text-field>
                <span class="v-input error--text v-messages">
                  <span v-if="hasHouseUnitBuildingNo">
                    Please enter a House/Unit/Building no.
                  </span>
                </span>
              </v-flex>
              <v-flex xs12 px-2>
                <v-text-field
                  v-model="personalInfo.StreetBarangay"
                  label="Street and Barangay*"
                  hide-details
                  required
                ></v-text-field>
                <span class="v-input error--text v-messages">
                  <span v-if="hasStreetBarangay">
                    Please enter a Street/Barangay
                  </span>
                </span>
              </v-flex>
              <v-flex xs12 px-2>
                <v-text-field
                  v-model="personalInfo.Landmark"
                  label="Landmark (i.e. near ABC Store)"
                  hide-details
                  required
                ></v-text-field>
              </v-flex>
            </v-layout>
            <small>*Indicates required field</small>
          </v-card-text>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="grey darken-1" flat @click="ResetHomeAddressField"
              >Close</v-btn
            >
            <v-btn color="blue darken-1" flat @click="ValidateHomeAddress"
              >OK</v-btn
            >
          </v-card-actions>
        </v-card>
      </v-dialog>
      <v-flex xs12 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Previous Address <b class="red--text">*</b></b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="personalInfo.PreviousAddress"
              color="purple darken-2"
              label="Please enter Previous Address"
              single-line
              outline
              clearable
              :rules="rPersonalInfo.PreviousAddress"
              required
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Birthplace <b class="red--text">*</b></b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="personalInfo.BirthPlace"
              color="purple darken-2"
              label="Birth Place"
              single-line
              outline
              clearable
              :rules="rPersonalInfo.BirthPlace"
              required
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Cellphone Number <b class="red--text">*</b></b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="personalInfo.Phone"
              color="purple darken-2"
              label="Cellphone Number"
              single-line
              outline
              clearable
              :rules="validateNumber"
              required
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Telephone Number</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="personalInfo.Telephone"
              color="purple darken-2"
              label="Telephone Number"
              single-line
              outline
              clearable
              :rules="validateNumber"
              required
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Email Address <b class="red--text">*</b></b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="personalInfo.Email"
              color="purple darken-2"
              label="email@domain.com"
              single-line
              outline
              clearable
              :rules="rPersonalInfo.Email"
              required
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Facebook Account</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="personalInfo.Facebook"
              color="purple darken-2"
              label="facebook.com/"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
    </v-layout>
    <v-layout row>
      <v-divider></v-divider>
    </v-layout>
    <v-layout row wrap justify-start>
      <v-flex xs12 sm4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2"
              >Stability of Residence <b class="red--text">*</b></b
            >
          </v-card-title>
          <v-container class="mt-4">
            <v-radio-group
              v-model="personalInfo.Residence"
              :rules="rPersonalInfo.Residence"
              @change="ResidenceChange"
              required
              :disabled="!isEdit"
            >
              <v-radio label="Owned" value="Owned"></v-radio>
              <v-radio label="Rented" value="Rented"></v-radio>
              <v-radio
                label="Living with Parents"
                value="Living With Parents"
              ></v-radio>
              <v-radio label="Free Housing" value="Free Housing"></v-radio>
            </v-radio-group>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm8 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2"
              >If Renting
              <b class="red--text" v-if="personalInfo.Residence == 'Rented'"
                >*</b
              ></b
            >
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="personalInfo.RentingName"
              label="Landlord Name"
              single-line
              outline
              clearable
              :rules="rPersonalInfo.RentingName"
              :disabled="!isEdit"
            ></v-text-field>
            <v-text-field
              v-model="personalInfo.RentingAddress"
              label="Address"
              single-line
              outline
              clearable
              :rules="rPersonalInfo.RentingAddress"
              :disabled="!isEdit"
            ></v-text-field>
            <v-text-field
              v-model="personalInfo.RentingTelNo"
              :rules="validateNumber"
              label="Tel No."
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Marital Status <b class="red--text">*</b></b>
          </v-card-title>
          <v-container class="mt-4">
            <v-radio-group
              justify-center
              v-model="personalInfo.Status"
              :rules="rPersonalInfo.Status"
              @change="MaritalStatusChange"
              :disabled="!isEdit"
              required
            >
              <v-radio label="Single" value="Single"></v-radio>
              <v-radio label="Married" value="Married"></v-radio>
              <v-radio label="Widow" value="Widow"></v-radio>
              <v-radio label="Separated" value="Separated"></v-radio>
              <v-radio
                label="Live-in or Common Law Partner"
                value="Live-in or Common Law Partner"
              ></v-radio>
            </v-radio-group>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Land <b class="red--text">*</b></b>
          </v-card-title>
          <v-container class="mt-4">
            <v-radio-group
              justify-center
              v-model="personalInfo.Land"
              :rules="rPersonalInfo.Land"
              required
              :disabled="!isEdit"
            >
              <v-radio label="Owned" value="owned"></v-radio>
              <v-radio label="Government" value="government"></v-radio>
              <v-radio label="Mortgage" value="mortgage"></v-radio>
            </v-radio-group>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">House Made <b class="red--text">*</b></b>
          </v-card-title>
          <v-container class="mt-4">
            <v-radio-group
              justify-center
              v-model="personalInfo.HouseMade"
              :rules="rPersonalInfo.HouseMade"
              required
              :disabled="!isEdit"
            >
              <v-radio label="Concrete" value="concrete"></v-radio>
              <v-radio label="Wood" value="wood"></v-radio>
            </v-radio-group>
          </v-container>
        </v-card>
      </v-flex>
    </v-layout>
    <!-- title - source of income -->
    <v-layout mt-5 row class="title-bg">
      <v-card flat>
        <h2 class="title-header">Source of Income</h2>
      </v-card>
    </v-layout>
    <v-layout row>
      <v-divider></v-divider>
    </v-layout>
    <!-- select source of income -->
    <v-layout column>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Industry</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-select
              v-model="industry"
              :items="selectionIndustry"
              solo
              color="black"
              label="Select"
              :rules="rIndustry"
              :disabled="!isEdit"
            ></v-select>
          </v-container>
        </v-card>
      </v-flex>
      <v-layout wrap>
        <v-checkbox
          label="Employed"
          value="employed"
          v-model="cbSOI"
          class="mr-5"
          :rules="hasSelectSOI"
          hide-details
          :disabled="!isEdit"
        ></v-checkbox>
        <v-checkbox
          label="Self-Employed"
          value="selfEmployed"
          v-model="cbSOI"
          class="mr-5"
          :rules="hasSelectSOI"
          hide-details
          :disabled="!isEdit"
        ></v-checkbox>
        <v-checkbox
          label="Own Business"
          value="ownBusiness"
          v-model="cbSOI"
          class="mr-5"
          :rules="hasSelectSOI"
          hide-details
          :disabled="!isEdit"
        ></v-checkbox>
        <v-checkbox
          label="Pension"
          value="pension"
          v-model="cbSOI"
          class="mr-5"
          :rules="hasSelectSOI"
          hide-details
          :disabled="!isEdit"
        ></v-checkbox>
        <v-checkbox
          label="Remittance"
          value="remittance"
          v-model="cbSOI"
          class="mr-5"
          :rules="hasSelectSOI"
          hide-details
          :disabled="!isEdit"
        ></v-checkbox>
      </v-layout>
      <span v-if="cbSOI.length <= 0" class="v-messages error--text"
        >At least one item should be selected</span
      >
    </v-layout>
    <!-- employed -->
    <v-layout row wrap justify-start v-if="cbSOI.includes('employed')">
      <v-flex xs12><h3 class="headline text-center">Employed</h3></v-flex>
      <v-flex xs12>
        <v-divider></v-divider>
      </v-flex>
      <v-flex xs12>
        <v-radio-group
          v-model="employed.TypeOf"
          row
          xs12
          :rules="rEmployed.TypeOf"
          required
          :disabled="!isEdit"
        >
          <v-radio label="Private" value="private"></v-radio>
          <v-radio label="Government" value="government"></v-radio>
        </v-radio-group>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Where Employed (Present) </b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="employed.Present"
              :rules="rEmployed.Present"
              color="purple darken-2"
              label="Where Employed (Present)"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">How Long (months)</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              :rules="rEmployed.HowLong"
              v-model="employed.HowLong"
              color="purple darken-2"
              label="How Long (months)"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Position</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              :rules="rEmployed.Position"
              v-model="employed.Position"
              color="purple darken-2"
              label="Position"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Present Business Address</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              :rules="rEmployed.PresentAddress"
              v-model="employed.PresentAddress"
              color="purple darken-2"
              label="Present Business Address"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Telephone Number</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="employed.Telephone"
              :rules="validateNumber"
              color="purple darken-2"
              label="Telephone Number"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Salary per Month</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="employed.Salary"
              :rules="rEmployed.Salary"
              color="purple darken-2"
              label="Salary per Month"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Previous Employment (if applicable) </b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="employed.Previous"
              color="purple darken-2"
              label="Previous Employment (if applicable)"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Previous Business Address (if applicable) </b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="employed.PreviousAddress"
              color="purple darken-2"
              label="Previous Business Address (if applicable)"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
    </v-layout>
    <!-- self employed/own business -->
    <v-layout
      row
      wrap
      justify-start
      v-if="cbSOI.includes('selfEmployed') || cbSOI.includes('ownBusiness')"
    >
      <v-flex
        xs12
        v-if="cbSOI.includes('selfEmployed') && cbSOI.includes('ownBusiness')"
        ><h3 class="headline text-center">
          Self-Employed / Own Business
        </h3></v-flex
      >
      <v-flex xs12 v-else-if="cbSOI.includes('selfEmployed')"
        ><h3 class="headline text-center">Self-Employed</h3></v-flex
      >
      <v-flex xs12 v-else-if="cbSOI.includes('ownBusiness')"
        ><h3 class="headline text-center">Own Business</h3></v-flex
      >
      <v-flex xs12>
        <v-divider></v-divider>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Nature of Business</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-select
              :items="selectionBusinessNature"
              v-model="selfOwn.BusinessNature"
              solo
              color="black"
              label="Select"
              :rules="rSelfOwn.BusinessNature"
              :disabled="!isEdit"
            ></v-select>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Business Name</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="selfOwn.BusinessName"
              color="purple darken-2"
              label="Business Name"
              single-line
              outline
              clearable
              :rules="rSelfOwn.BusinessName"
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Business Address</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="selfOwn.BusinessAddress"
              color="purple darken-2"
              label="Business Address"
              single-line
              outline
              clearable
              :rules="rSelfOwn.BusinessAddress"
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Capital</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="selfOwn.Capital"
              color="purple darken-2"
              label="P 00,000.00"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Monthly Income</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="selfOwn.Net"
              color="purple darken-2"
              label="Monthly Income (Net)"
              single-line
              outline
              clearable
            ></v-text-field>
            <v-text-field
              v-model="selfOwn.Gross"
              color="purple darken-2"
              label="Monthly Income (Gross)"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Phone Number</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="selfOwn.PhoneNumber"
              :rules="validateNumber"
              color="purple darken-2"
              label="Phone Number"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">How Long</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="selfOwn.HowLong"
              color="purple darken-2"
              label="How Long"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
    </v-layout>
    <!-- cbPension -->
    <v-layout row wrap justify-start v-if="cbSOI.includes('pension')">
      <v-flex xs12><h3 class="headline text-center">Pension</h3></v-flex>
      <v-flex xs12>
        <v-divider></v-divider>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Agency of Pension</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="pension.Agency"
              :rules="rPension.Agency"
              color="purple darken-2"
              label="Agency of Pension"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Monthly Pension</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="pension.Monthly"
              :rules="rPension.Monthly"
              color="purple darken-2"
              label="Monthly Pension"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
    </v-layout>
    <!-- cbRemittance -->
    <v-layout row wrap justify-start v-if="cbSOI.includes('remittance')">
      <v-flex xs12><h3 class="headline text-center">Remittance</h3></v-flex>
      <v-flex xs12>
        <v-divider></v-divider>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Name of Remitter</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="remittance.Name"
              :rules="rRemittance.Name"
              color="purple darken-2"
              label="Name of Remitter"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Location of Remitter</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="remittance.Location"
              :rules="rRemittance.Location"
              color="purple darken-2"
              label="Location of Remitter"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Relationship</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-select
              v-model="remittance.Relationship"
              :rules="rRemittance.Relationship"
              solo
              :items="selectionRelationship"
              color="black"
              label="Select"
              :disabled="!isEdit"
            ></v-select>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Monthly Remittance Amount </b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="remittance.MonthlyAmount"
              :rules="rRemittance.MonthlyAmount"
              color="purple darken-2"
              label="Monthly Remittance Amount"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Frequency of Remittance </b>
          </v-card-title>
          <v-container class="mt-4">
            <v-select
              v-model="remittance.Frequency"
              :rules="rRemittance.Frequency"
              solo
              :items="selectionFrequency"
              color="black"
              label="Select"
              :disabled="!isEdit"
            ></v-select>
          </v-container>
        </v-card>
      </v-flex>
    </v-layout>
    <!-- title - Parents -->
    <v-layout mt-5 row class="title-bg">
      <v-card flat>
        <h2 class="title-header">Parents</h2>
      </v-card>
    </v-layout>
    <v-layout row>
      <v-divider></v-divider>
    </v-layout>
    <!-- Parents -->
    <v-layout row wrap justify-start>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Father's Name <b class="red--text">*</b></b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="parents.Father.Name"
              color="purple darken-2"
              label="Father's Name"
              single-line
              outline
              clearable
              :rules="rParents.Father.Name"
              required
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Age <b class="red--text">*</b></b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="parents.Father.Age"
              color="purple darken-2"
              label="Age"
              single-line
              outline
              clearable
              :rules="rParents.Father.Age"
              required
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Address <b class="red--text">*</b></b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="parents.Father.Address"
              color="purple darken-2"
              label="Address"
              single-line
              outline
              clearable
              :rules="rParents.Father.Address"
              required
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Facebook Account</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="parents.Father.Facebook"
              color="purple darken-2"
              label="facebook.com/"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Employment/Source of Income </b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="parents.Father.Employment"
              color="purple darken-2"
              label="Employment/Source of Income"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Office Address</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="parents.Father.OfficeAddress"
              color="purple darken-2"
              label="Office Address"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Position</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="parents.Father.Position"
              color="purple darken-2"
              label="Position"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">How Long</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="parents.Father.HowLong"
              color="purple darken-2"
              label="How Long"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
    </v-layout>
    <v-layout row>
      <v-divider></v-divider>
    </v-layout>
    <v-layout row wrap justify-start>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2"
              >Mother's Maiden Name <b class="red--text">*</b></b
            >
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="parents.Mother.Name"
              color="purple darken-2"
              label="Mother's Maiden Name"
              single-line
              outline
              clearable
              :rules="rParents.Mother.Name"
              required
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Age <b class="red--text">*</b></b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="parents.Mother.Age"
              color="purple darken-2"
              label="Age"
              single-line
              outline
              clearable
              :rules="rParents.Mother.Age"
              required
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Address <b class="red--text">*</b></b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="parents.Mother.Address"
              color="purple darken-2"
              label="Address"
              single-line
              outline
              clearable
              :rules="rParents.Mother.Address"
              required
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Facebook Account</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="parents.Mother.Facebook"
              color="purple darken-2"
              label="facebook.com/"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Employment/Source of Income </b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="parents.Mother.Employment"
              color="purple darken-2"
              label="Employment/Source of Income"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Office Address</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="parents.Mother.OfficeAddress"
              color="purple darken-2"
              label="Office Address"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Position</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="parents.Mother.Position"
              color="purple darken-2"
              label="Position"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">How Long</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="parents.Mother.HowLong"
              color="purple darken-2"
              label="How Long"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
    </v-layout>
    <!-- title - Spouse -->
    <v-layout mt-5 row class="title-bg">
      <v-card flat>
        <h2 class="title-flex title-header">
          Spouse
          <span class="font-italic white--text font-weight-thin subheading"
            >&nbsp;(Please skip this step if not applicable) &nbsp;</span
          >
          <v-btn
            @click="spouseExpandable"
            flat
            icon
            color="white"
            class="expandable-icon"
          >
            <v-icon v-if="skipSpouse">add</v-icon>
            <v-icon v-else>remove</v-icon>
          </v-btn>
        </h2>
      </v-card>
    </v-layout>
    <v-layout row>
      <v-divider></v-divider>
    </v-layout>
    <v-layout column v-if="!skipSpouse">
      <v-layout row wrap justify-start>
        <v-flex xs12 sm6 md4 pa-2>
          <v-card class="info-card">
            <v-card-title class="grey lighten-2 index-label">
              <b class="body-2">Spouse Name </b>
            </v-card-title>
            <v-container class="mt-4">
              <v-text-field
                v-model="spouse.Name"
                color="purple darken-2"
                label="Spouse Name"
                single-line
                outline
                clearable
                :rules="rSpouse.Name"
                :disabled="!isEdit"
              ></v-text-field>
            </v-container>
          </v-card>
        </v-flex>
        <v-flex xs12 sm6 md4 pa-2>
          <v-card class="info-card">
            <v-card-title class="grey lighten-2 index-label">
              <b class="body-2">Age </b>
            </v-card-title>
            <v-container class="mt-4">
              <v-text-field
                v-model="spouse.Age"
                color="purple darken-2"
                label="Age"
                single-line
                outline
                clearable
                :rules="rSpouse.Age"
                :disabled="!isEdit"
              ></v-text-field>
            </v-container>
          </v-card>
        </v-flex>
        <v-flex xs12 sm6 md4 pa-2>
          <v-card class="info-card">
            <v-card-title class="grey lighten-2 index-label">
              <b class="body-2">Source of Income </b>
            </v-card-title>
            <v-container class="mt-4">
              <v-text-field
                v-model="spouse.IncomeSource"
                color="purple darken-2"
                label="Source of Income"
                single-line
                outline
                clearable
                :disabled="!isEdit"
              ></v-text-field>
            </v-container>
          </v-card>
        </v-flex>
        <v-flex xs12 sm6 md4 pa-2>
          <v-card class="info-card">
            <v-card-title class="grey lighten-2 index-label">
              <b class="body-2">Office Address </b>
            </v-card-title>
            <v-container class="mt-4">
              <v-text-field
                v-model="spouse.OfficeAddress"
                color="purple darken-2"
                label="Office Address"
                single-line
                outline
                clearable
                :disabled="!isEdit"
              ></v-text-field>
            </v-container>
          </v-card>
        </v-flex>
        <v-flex xs12 sm6 md4 pa-2>
          <v-card class="info-card">
            <v-card-title class="grey lighten-2 index-label">
              <b class="body-2">Position</b>
            </v-card-title>
            <v-container class="mt-4">
              <v-text-field
                v-model="spouse.Position"
                color="purple darken-2"
                label="Position"
                single-line
                outline
                clearable
                :disabled="!isEdit"
              ></v-text-field>
            </v-container>
          </v-card>
        </v-flex>
        <v-flex xs12 sm6 md4 pa-2>
          <v-card class="info-card">
            <v-card-title class="grey lighten-2 index-label">
              <b class="body-2">How Long</b>
            </v-card-title>
            <v-container class="mt-4">
              <v-text-field
                v-model="spouse.HowLong"
                color="purple darken-2"
                label="How Long"
                single-line
                outline
                clearable
                :disabled="!isEdit"
              ></v-text-field>
            </v-container>
          </v-card>
        </v-flex>
        <v-flex xs12 sm6 md4 pa-2>
          <v-card class="info-card">
            <v-card-title class="grey lighten-2 index-label">
              <b class="body-2">Office Tel. No.</b>
            </v-card-title>
            <v-container class="mt-4">
              <v-text-field
                v-model="spouse.OfficeTelNo"
                :rules="validateNumber"
                color="purple darken-2"
                label="Office Tel. No."
                single-line
                outline
                clearable
                :disabled="!isEdit"
              ></v-text-field>
            </v-container>
          </v-card>
        </v-flex>
        <v-flex xs12 sm6 md4 pa-2>
          <v-card class="info-card">
            <v-card-title class="grey lighten-2 index-label">
              <b class="body-2">Salary per Month</b>
            </v-card-title>
            <v-container class="mt-4">
              <v-text-field
                v-model="spouse.Salary"
                color="purple darken-2"
                label="Salary per Month"
                single-line
                outline
                clearable
                :disabled="!isEdit"
              ></v-text-field>
            </v-container>
          </v-card>
        </v-flex>
      </v-layout>
      <v-layout row>
        <v-divider></v-divider>
      </v-layout>
    </v-layout>
    <!-- title - Parents-in-Law -->
    <v-layout mt-5 row class="title-bg">
      <v-card flat>
        <h2 class="title-flex title-header">
          Parents-in-Laws
          <span class="font-italic white--text font-weight-thin subheading"
            >&nbsp;(Please skip this step if not applicable) &nbsp;</span
          >
          <v-btn
            @click="pilExpandable"
            flat
            icon
            color="white"
            class="expandable-icon"
          >
            <v-icon v-if="skipParentLaw">add</v-icon>
            <v-icon v-else>remove</v-icon>
          </v-btn>
        </h2>
      </v-card>
    </v-layout>
    <v-layout row>
      <v-divider></v-divider>
    </v-layout>

    <v-layout column v-if="!skipParentLaw">
      <!-- father-in-law -->
      <v-layout row wrap justify-start>
        <v-flex xs12 sm6 md4 pa-2>
          <v-card class="info-card">
            <v-card-title class="grey lighten-2 index-label">
              <b class="body-2">Father-In-Law's Name </b>
            </v-card-title>
            <v-container class="mt-4">
              <v-text-field
                v-model="parentsInLaw.Father.Name"
                :rules="rParentsInLaw.Father.Name"
                color="purple darken-2"
                label="Father-In-Law's Name"
                single-line
                outline
                clearable
                :disabled="!isEdit"
              ></v-text-field>
            </v-container>
          </v-card>
        </v-flex>
        <v-flex xs12 sm6 md4 pa-2>
          <v-card class="info-card">
            <v-card-title class="grey lighten-2 index-label">
              <b class="body-2">Age </b>
            </v-card-title>
            <v-container class="mt-4">
              <v-text-field
                v-model="parentsInLaw.Father.Age"
                :rules="rParentsInLaw.Father.Age"
                color="purple darken-2"
                label="Age"
                single-line
                outline
                clearable
                :disabled="!isEdit"
              ></v-text-field>
            </v-container>
          </v-card>
        </v-flex>
        <v-flex xs12 sm6 md4 pa-2>
          <v-card class="info-card">
            <v-card-title class="grey lighten-2 index-label">
              <b class="body-2">Address </b>
            </v-card-title>
            <v-container class="mt-4">
              <v-text-field
                v-model="parentsInLaw.Father.Address"
                :rules="rParentsInLaw.Father.Address"
                color="purple darken-2"
                label="Address"
                single-line
                outline
                clearable
                :disabled="!isEdit"
              ></v-text-field>
            </v-container>
          </v-card>
        </v-flex>
        <v-flex xs12 sm6 md4 pa-2>
          <v-card class="info-card">
            <v-card-title class="grey lighten-2 index-label">
              <b class="body-2">Facebook Account</b>
            </v-card-title>
            <v-container class="mt-4">
              <v-text-field
                v-model="parentsInLaw.Father.Facebook"
                color="purple darken-2"
                label="facebook.com/"
                single-line
                outline
                clearable
                :disabled="!isEdit"
              ></v-text-field>
            </v-container>
          </v-card>
        </v-flex>
        <v-flex xs12 sm6 md4 pa-2>
          <v-card class="info-card">
            <v-card-title class="grey lighten-2 index-label">
              <b class="body-2">Employment/Source of Income </b>
            </v-card-title>
            <v-container class="mt-4">
              <v-text-field
                v-model="parentsInLaw.Father.Employment"
                color="purple darken-2"
                label="Employment/Source of Income"
                single-line
                outline
                clearable
                :disabled="!isEdit"
              ></v-text-field>
            </v-container>
          </v-card>
        </v-flex>
        <v-flex xs12 sm6 md4 pa-2>
          <v-card class="info-card">
            <v-card-title class="grey lighten-2 index-label">
              <b class="body-2">Office Address</b>
            </v-card-title>
            <v-container class="mt-4">
              <v-text-field
                v-model="parentsInLaw.Father.OfficeAddress"
                color="purple darken-2"
                label="Office Address"
                single-line
                outline
                clearable
                :disabled="!isEdit"
              ></v-text-field>
            </v-container>
          </v-card>
        </v-flex>
        <v-flex xs12 sm6 md4 pa-2>
          <v-card class="info-card">
            <v-card-title class="grey lighten-2 index-label">
              <b class="body-2">Position</b>
            </v-card-title>
            <v-container class="mt-4">
              <v-text-field
                v-model="parentsInLaw.Father.Position"
                color="purple darken-2"
                label="Position"
                single-line
                outline
                clearable
                :disabled="!isEdit"
              ></v-text-field>
            </v-container>
          </v-card>
        </v-flex>
        <v-flex xs12 sm6 md4 pa-2>
          <v-card class="info-card">
            <v-card-title class="grey lighten-2 index-label">
              <b class="body-2">How Long</b>
            </v-card-title>
            <v-container class="mt-4">
              <v-text-field
                v-model="parentsInLaw.Father.HowLong"
                color="purple darken-2"
                label="How Long"
                single-line
                outline
                clearable
                :disabled="!isEdit"
              ></v-text-field>
            </v-container>
          </v-card>
        </v-flex>
      </v-layout>
      <v-layout row>
        <v-divider></v-divider>
      </v-layout>
      <v-layout row wrap justify-start>
        <v-flex xs12 sm6 md4 pa-2>
          <v-card class="info-card">
            <v-card-title class="grey lighten-2 index-label">
              <b class="body-2">Mother-In-Law's Name </b>
            </v-card-title>
            <v-container class="mt-4">
              <v-text-field
                v-model="parentsInLaw.Mother.Name"
                :rules="rParentsInLaw.Mother.Name"
                color="purple darken-2"
                label="Mother-In-Law's Name"
                single-line
                outline
                clearable
                :disabled="!isEdit"
              ></v-text-field>
            </v-container>
          </v-card>
        </v-flex>
        <v-flex xs12 sm6 md4 pa-2>
          <v-card class="info-card">
            <v-card-title class="grey lighten-2 index-label">
              <b class="body-2">Age </b>
            </v-card-title>
            <v-container class="mt-4">
              <v-text-field
                v-model="parentsInLaw.Mother.Age"
                :rules="rParentsInLaw.Mother.Age"
                color="purple darken-2"
                label="Age"
                single-line
                outline
                clearable
                :disabled="!isEdit"
              ></v-text-field>
            </v-container>
          </v-card>
        </v-flex>
        <v-flex xs12 sm6 md4 pa-2>
          <v-card class="info-card">
            <v-card-title class="grey lighten-2 index-label">
              <b class="body-2">Address </b>
            </v-card-title>
            <v-container class="mt-4">
              <v-text-field
                v-model="parentsInLaw.Mother.Address"
                :rules="rParentsInLaw.Mother.Address"
                color="purple darken-2"
                label="Address"
                single-line
                outline
                clearable
                :disabled="!isEdit"
              ></v-text-field>
            </v-container>
          </v-card>
        </v-flex>
        <v-flex xs12 sm6 md4 pa-2>
          <v-card class="info-card">
            <v-card-title class="grey lighten-2 index-label">
              <b class="body-2">Facebook Account</b>
            </v-card-title>
            <v-container class="mt-4">
              <v-text-field
                v-model="parentsInLaw.Mother.Facebook"
                color="purple darken-2"
                label="facebook.com/"
                single-line
                outline
                clearable
                :disabled="!isEdit"
              ></v-text-field>
            </v-container>
          </v-card>
        </v-flex>
        <v-flex xs12 sm6 md4 pa-2>
          <v-card class="info-card">
            <v-card-title class="grey lighten-2 index-label">
              <b class="body-2">Employment/Source of Income </b>
            </v-card-title>
            <v-container class="mt-4">
              <v-text-field
                v-model="parentsInLaw.Mother.Employment"
                color="purple darken-2"
                label="Employment/Source of Income"
                single-line
                outline
                clearable
                :disabled="!isEdit"
              ></v-text-field>
            </v-container>
          </v-card>
        </v-flex>
        <v-flex xs12 sm6 md4 pa-2>
          <v-card class="info-card">
            <v-card-title class="grey lighten-2 index-label">
              <b class="body-2">Office Address</b>
            </v-card-title>
            <v-container class="mt-4">
              <v-text-field
                v-model="parentsInLaw.Mother.OfficeAddress"
                color="purple darken-2"
                label="Office Address"
                single-line
                outline
                clearable
                :disabled="!isEdit"
              ></v-text-field>
            </v-container>
          </v-card>
        </v-flex>
        <v-flex xs12 sm6 md4 pa-2>
          <v-card class="info-card">
            <v-card-title class="grey lighten-2 index-label">
              <b class="body-2">Position</b>
            </v-card-title>
            <v-container class="mt-4">
              <v-text-field
                v-model="parentsInLaw.Mother.Position"
                color="purple darken-2"
                label="Position"
                single-line
                outline
                clearable
                :disabled="!isEdit"
              ></v-text-field>
            </v-container>
          </v-card>
        </v-flex>
        <v-flex xs12 sm6 md4 pa-2>
          <v-card class="info-card">
            <v-card-title class="grey lighten-2 index-label">
              <b class="body-2">How Long</b>
            </v-card-title>
            <v-container class="mt-4">
              <v-text-field
                v-model="parentsInLaw.Mother.HowLong"
                color="purple darken-2"
                label="How Long"
                single-line
                outline
                clearable
                :disabled="!isEdit"
              ></v-text-field>
            </v-container>
          </v-card>
        </v-flex>
      </v-layout>
    </v-layout>
    <!-- title - Children -->
    <v-layout mt-5 row class="title-bg">
      <v-card flat>
        <h2 class="title-flex title-header">
          Children
          <span class="font-italic white--text font-weight-thin subheading"
            >&nbsp;(Please skip this step if not applicable) &nbsp;
          </span>
          <v-btn
            @click="childExpandable"
            flat
            icon
            color="white"
            class="expandable-icon"
          >
            <v-icon v-if="skipChildren">add</v-icon>
            <v-icon v-else>remove</v-icon>
          </v-btn>
        </h2>
      </v-card>
    </v-layout>
    <v-layout row>
      <v-divider></v-divider>
    </v-layout>
    <v-layout column mt-3 v-if="!skipChildren">
      <v-flex xs12 class="table-responsive">
        <table>
          <thead>
            <tr>
              <th width="15%">Child's Name</th>
              <th width="5%">Age</th>
              <th width="15%">Home Address</th>
              <th width="10%">Telephone #</th>
              <th width="10%">Employment/School</th>
              <th width="15%">Office/School Address</th>
              <th width="10%">Position/Grade</th>
              <th width="10%">How Long</th>
              <th width="10%">Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(child, index) in listChildren" :key="index">
              <td width="15%">
                {{ child.ChildName }}
              </td>
              <td width="5%">
                {{ child.ChildAge }}
              </td>
              <td width="15%">
                {{ child.ChildHomeAddress }}
              </td>
              <td width="10%">
                {{ child.ChildTelNo }}
              </td>
              <td width="10%">
                {{ child.ChildEmploySchool }}
              </td>
              <td width="15%">
                {{ child.ChildPosGrade }}
              </td>
              <td width="10%">
                {{ child.ChildEmploySchoolAddress }}
              </td>
              <td width="10%">
                {{ child.ChildHowLong }}
              </td>
              <td width="10%">
                <v-layout justify-center align-center>
                  <v-btn
                    @click="delChildRow(index)"
                    class="mx-2 actions-question btn_primary"
                    fab
                    small
                    :disabled="isAddChildren || !isEdit"
                  >
                    <v-icon>delete</v-icon>
                  </v-btn>
                  <v-btn
                    @click="editChild(index, child)"
                    class="mx-2 actions-question"
                    fab
                    small
                    color="#1332A8"
                    :disabled="isAddChildren || !isEdit"
                  >
                    <v-icon class="check-icon">edit</v-icon>
                  </v-btn>
                </v-layout>
              </td>
            </tr>
            <v-dialog v-model="isAddChildren" persistent max-width="600px">
              <v-card>
                <v-card-title
                  class="font-weight-medium grey lighten-2"
                  primary-title
                >
                  Add Children
                </v-card-title>
                <v-card-text>
                  <v-container grid-list-md>
                    <v-layout wrap>
                      <v-flex xs12 sm8>
                        <v-text-field
                          v-model="children.ChildName"
                          value=""
                          label="Child's Name*"
                          hide-details
                          required
                          :disabled="!isEdit"
                        ></v-text-field>
                        <span class="v-input error--text v-messages">
                          <span v-if="hasChildName">
                            Please enter Child's Name
                          </span>
                        </span>
                      </v-flex>
                      <v-flex xs12 sm4>
                        <v-text-field
                          v-model="children.ChildAge"
                          value=""
                          label="Age*"
                          @keypress="NumbersOnly"
                          hide-details
                          required
                          :disabled="!isEdit"
                        ></v-text-field>
                        <span class="v-input error--text v-messages">
                          <span v-if="hasChildAge">
                            Please enter Age
                          </span>
                        </span>
                      </v-flex>
                      <v-flex xs12 sm8>
                        <v-text-field
                          v-model="children.ChildHomeAddress"
                          value=""
                          label="Home Address*"
                          hide-details
                          required
                          :disabled="!isEdit"
                        ></v-text-field>
                        <span class="v-input error--text v-messages">
                          <span v-if="hasChildHomeAddress">
                            Please enter Home Address
                          </span>
                        </span>
                      </v-flex>
                      <v-flex xs12 sm4>
                        <v-text-field
                          v-model="children.ChildTelNo"
                          value=""
                          label="Telephone*"
                          hide-details
                          required
                          :disabled="!isEdit"
                        ></v-text-field>
                        <span class="v-input error--text v-messages">
                          <span v-if="hasChildTelephone">
                            Please enter Telephone Number
                          </span>
                        </span>
                      </v-flex>
                      <v-flex xs12 sm8>
                        <v-text-field
                          v-model="children.ChildEmploySchool"
                          value=""
                          label="Employment Or School*"
                          hide-details
                          required
                          :disabled="!isEdit"
                        ></v-text-field>
                        <span class="v-input error--text v-messages">
                          <span v-if="hasChildEmploymentOrSchool">
                            Please enter Employment or School
                          </span>
                        </span>
                      </v-flex>
                      <v-flex xs12 sm4>
                        <v-text-field
                          v-model="children.ChildPosGrade"
                          value=""
                          label="Position Or Grade*"
                          hide-details
                          required
                          :disabled="!isEdit"
                        ></v-text-field>
                        <span class="v-input error--text v-messages">
                          <span v-if="hasChildPositionOrGrade">
                            Please enter Position or Grade
                          </span>
                        </span>
                      </v-flex>
                      <v-flex xs12 sm8>
                        <v-text-field
                          v-model="children.ChildEmploySchoolAddress"
                          value=""
                          label="Employment Or School Address*"
                          hide-details
                          required
                          :disabled="!isEdit"
                        ></v-text-field>
                        <span class="v-input error--text v-messages">
                          <span v-if="hasChildAddress">
                            Please enter School/Employment Address
                          </span>
                        </span>
                      </v-flex>
                      <v-flex xs12 sm4>
                        <v-text-field
                          v-model="children.ChildHowLong"
                          value=""
                          label="How Long*"
                          hide-details
                          required
                          :disabled="!isEdit"
                        ></v-text-field>
                        <span class="v-input error--text v-messages">
                          <span v-if="hasChildHowLong">
                            Please enter How Long
                          </span>
                        </span>
                      </v-flex>
                    </v-layout>
                  </v-container>
                  <small>*Indicates required field</small>
                  <div><small>Please put ( - ) if not applicable</small></div>
                </v-card-text>
                <v-card-actions>
                  <v-spacer></v-spacer>
                  <v-btn color="grey darken-1" flat @click="toggleChildModal"
                    >Close</v-btn
                  >
                  <v-btn
                    v-if="isEditChildren"
                    color="blue darken-1"
                    flat
                    @click="saveChild(indexEditChildren, children)"
                    >Save</v-btn
                  >
                  <v-btn
                    v-else
                    color="blue darken-1"
                    flat
                    @click="addChild(children)"
                    >Add</v-btn
                  >
                </v-card-actions>
              </v-card>
            </v-dialog>
          </tbody>
        </table>
      </v-flex>
      <v-flex align-self-end>
        <v-btn
          @click="toggleChildModal"
          class="btn_primary"
          :disabled="isAddChildren || !isEdit"
        >
          <v-icon>add</v-icon>
          Add Children
        </v-btn>
      </v-flex>
    </v-layout>
    <!-- Credit History -->
    <v-layout mt-5 row class="title-bg">
      <v-card flat>
        <h2 class="title-header">
          Credit History with Other Financing Institutions
        </h2>
      </v-card>
    </v-layout>
    <v-layout row>
      <v-divider></v-divider>
    </v-layout>
    <v-layout column mt-3>
      <v-flex xs12 class="table-responsive">
        <table>
          <thead>
            <tr>
              <th width="25%">Company Name</th>
              <th width="15%">Type of Unit</th>
              <th width="20%">Date of Purchase</th>
              <th width="15%">Terms</th>
              <th width="15%">Remaining Balance</th>
              <th width="10%">Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(history, index) in listCreditHistory" :key="index">
              <td width="25%">
                {{ history.HistoryCompanyName }}
              </td>
              <td width="15%">
                {{ history.HistoryTypeOfUnit }}
              </td>
              <td width="20%">
                {{ history.HistoryDatePurchase }}
              </td>
              <td width="15%">
                {{ history.HistoryTerms }}
              </td>
              <td width="15%">
                {{ history.HistoryRemainingBalance }}
              </td>
              <td width="10%">
                <v-layout justify-center align-center>
                  <v-btn
                    @click="delCreditHistoryRow(index)"
                    class="mx-2 actions-question btn_primary"
                    fab
                    small
                    :disabled="isAddCreditHistory || !isEdit"
                  >
                    <v-icon>delete</v-icon>
                  </v-btn>
                  <v-btn
                    @click="editCreditHistory(index, history)"
                    class="mx-2 actions-question"
                    fab
                    small
                    color="#1332A8"
                    :disabled="isAddCreditHistory || !isEdit"
                  >
                    <v-icon class="check-icon">edit</v-icon>
                  </v-btn>
                </v-layout>
              </td>
            </tr>
            <v-dialog v-model="isAddCreditHistory" persistent max-width="600px">
              <v-card>
                <v-card-title
                  class="font-weight-medium grey lighten-2"
                  primary-title
                >
                  Add Credit History
                </v-card-title>
                <v-card-text>
                  <v-container grid-list-md>
                    <v-layout wrap>
                      <v-flex xs12>
                        <v-text-field
                          v-model="creditHistory.HistoryCompanyName"
                          value=""
                          label="Company Name*"
                          hide-details
                          required
                          :disabled="!isEdit"
                        ></v-text-field>
                        <span class="v-input error--text v-messages">
                          <span v-if="hasCreditHistoryName">
                            Please enter Company Name
                          </span>
                        </span>
                      </v-flex>
                      <v-flex xs12 sm6>
                        <v-text-field
                          v-model="creditHistory.HistoryTypeOfUnit"
                          value=""
                          label="Type of Unit*"
                          hide-details
                          required
                          :disabled="!isEdit"
                        ></v-text-field>
                        <span class="v-input error--text v-messages">
                          <span v-if="hasCreditHistoryType">
                            Please enter Type of Unit
                          </span>
                        </span>
                      </v-flex>
                      <v-flex xs12 sm6>
                        <v-menu
                          transition="scale-transition"
                          :close-on-content-click="false"
                          lazy
                          offset-y
                        >
                          <template v-slot:activator="{ on }">
                            <v-text-field
                              v-model="creditHistory.HistoryDatePurchase"
                              v-on="on"
                              append-icon="event"
                              readonly
                              color="red"
                              hide-details
                              class="px-2"
                              :disabled="!isEdit"
                            >
                            </v-text-field>
                          </template>
                          <v-date-picker
                            v-model="creditHistory.HistoryDatePurchase"
                            class="customTable"
                            color="red"
                          >
                          </v-date-picker>
                        </v-menu>
                        <span class="v-input error--text v-messages">
                          <span v-if="hasCreditHistoryDate">
                            Please choose Date of Purchase
                          </span>
                        </span>
                      </v-flex>
                      <v-flex xs12 sm6>
                        <v-text-field
                          v-model="creditHistory.HistoryTerms"
                          value=""
                          label="Terms*"
                          hide-details
                          :disabled="!isEdit"
                        ></v-text-field>
                        <span class="v-input error--text v-messages">
                          <span v-if="hasCreditHistoryTerms">
                            Please enter Terms
                          </span>
                        </span>
                      </v-flex>
                      <v-flex xs12 sm6>
                        <v-text-field
                          v-model="creditHistory.HistoryRemainingBalance"
                          value=""
                          label="Remaining Balance*"
                          hide-details
                          :disabled="!isEdit"
                        ></v-text-field>
                        <span class="v-input error--text v-messages">
                          <span v-if="hasCreditHistoryBalance">
                            Please enter Remaining Balance
                          </span>
                        </span>
                      </v-flex>
                    </v-layout>
                  </v-container>
                  <small>*Indicates required field</small>
                  <div><small>Please put ( - ) if not applicable</small></div>
                </v-card-text>
                <v-card-actions>
                  <v-spacer></v-spacer>
                  <v-btn
                    color="grey darken-1"
                    flat
                    @click="toggleCreditHistoryModal"
                    >Close</v-btn
                  >
                  <v-btn
                    v-if="isEditCreditHistory"
                    color="blue darken-1"
                    flat
                    @click="
                      saveCreditHistory(indexEditCreditHistory, creditHistory)
                    "
                    >Save</v-btn
                  >
                  <v-btn
                    v-else
                    color="blue darken-1"
                    flat
                    @click="addCreditHistory(creditHistory)"
                    >Add</v-btn
                  >
                </v-card-actions>
              </v-card>
            </v-dialog>
          </tbody>
        </table>
      </v-flex>
      <v-flex align-self-end>
        <v-btn
          @click="toggleCreditHistoryModal"
          class="btn_primary"
          :disabled="isAddCreditHistory || !isEdit"
        >
          <v-icon>add</v-icon>
          Add Credit History
        </v-btn>
      </v-flex>
    </v-layout>
    <!-- Personal Property Owned -->
    <v-layout mt-5 row class="title-bg">
      <v-card flat>
        <h2 class="title-header">Personal Property Owned</h2>
      </v-card>
    </v-layout>
    <v-layout row>
      <v-divider></v-divider>
    </v-layout>
    <v-layout column mt-3>
      <v-flex xs12 class="table-responsive">
        <table>
          <thead>
            <tr>
              <th width="90%">Property</th>
              <th width="10%">Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(properties, index) in listPropertyOwned" :key="index">
              <td width="90%">
                {{ properties.Property }}
              </td>
              <td width="10%">
                <v-layout justify-center align-center>
                  <v-btn
                    @click="delPropertyRow(index)"
                    class="mx-2 actions-question btn_primary"
                    fab
                    small
                    :disabled="isAddProperty || !isEdit"
                  >
                    <v-icon>delete</v-icon>
                  </v-btn>
                  <v-btn
                    @click="editProperty(index, properties)"
                    class="mx-2 actions-question"
                    fab
                    small
                    color="#1332A8"
                    :disabled="isAddProperty || !isEdit"
                  >
                    <v-icon class="check-icon">edit</v-icon>
                  </v-btn>
                </v-layout>
              </td>
            </tr>
            <v-dialog v-model="isAddProperty" persistent max-width="500px">
              <v-card>
                <v-card-title
                  class="font-weight-medium grey lighten-2"
                  primary-title
                >
                  Add Property
                </v-card-title>
                <v-card-text>
                  <v-container grid-list-md>
                    <v-layout wrap>
                      <v-flex xs12>
                        <v-text-field
                          v-model="property.Property"
                          value=""
                          label="Property*"
                          hide-details
                          required
                          :disabled="!isEdit"
                        ></v-text-field>
                        <span class="v-input error--text v-messages">
                          <span v-if="hasPropertyName">
                            Please enter Property
                          </span>
                        </span>
                      </v-flex>
                    </v-layout>
                  </v-container>
                  <small>*Indicates required field</small>
                </v-card-text>
                <v-card-actions>
                  <v-spacer></v-spacer>
                  <v-btn color="grey darken-1" flat @click="togglePropertyModal"
                    >Close</v-btn
                  >
                  <v-btn
                    v-if="isEditProperty"
                    color="blue darken-1"
                    flat
                    @click="saveProperty(indexEditProperty, property)"
                    >Save</v-btn
                  >
                  <v-btn
                    v-else
                    color="blue darken-1"
                    flat
                    @click="addProperty(property)"
                    >Add</v-btn
                  >
                </v-card-actions>
              </v-card>
            </v-dialog>
          </tbody>
        </table>
      </v-flex>
      <v-flex align-self-end>
        <v-btn
          @click="togglePropertyModal"
          class="btn_primary"
          :disabled="isAddProperty || !isEdit"
        >
          <v-icon>add</v-icon>
          Add Property
        </v-btn>
      </v-flex>
    </v-layout>
    <!-- Unit Desired -->
    <v-layout mt-5 row class="title-bg">
      <v-card flat>
        <h2 class="title-header">Unit(s) Desired</h2>
      </v-card>
    </v-layout>
    <v-layout row>
      <v-divider></v-divider>
    </v-layout>
    <v-layout column mt-3>
      <v-flex xs12 class="table-responsive">
        <table>
          <thead>
            <tr>
              <th width="15%">Brand Model</th>
              <th width="20%">Serial #</th>
              <th width="15%">Code</th>
              <th width="20%">Amount</th>
              <th width="20%">Accounting</th>
              <th width="10%">Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(desired, index) in listUnitDesired" :key="index">
              <td width="15%">
                {{ desired.DesiredBrandModel }}
              </td>
              <td width="20%">
                {{ desired.DesiredSerialNo }}
              </td>
              <td width="15%">
                {{ desired.DesiredCode }}
              </td>
              <td width="20%">
                {{ desired.DesiredAmount }}
              </td>
              <td width="20%">
                {{ desired.DesiredAccounting }}
              </td>
              <td width="10%">
                <v-layout justify-center align-center>
                  <v-btn
                    @click="delUnitDesiredRow(index)"
                    class="mx-2 actions-question btn_primary"
                    fab
                    small
                    :disabled="isAddUnitDesired || !isEdit"
                  >
                    <v-icon>delete</v-icon>
                  </v-btn>
                  <v-btn
                    @click="editUnitDesired(index, desired)"
                    class="mx-2 actions-question"
                    fab
                    small
                    color="#1332A8"
                    :disabled="isAddUnitDesired || !isEdit"
                  >
                    <v-icon class="check-icon">edit</v-icon>
                  </v-btn>
                </v-layout>
              </td>
            </tr>
            <v-dialog v-model="isAddUnitDesired" persistent max-width="600px">
              <v-card>
                <v-card-title
                  class="font-weight-medium grey lighten-2"
                  primary-title
                >
                  Add Unit Desired
                </v-card-title>
                <v-card-text>
                  <v-container grid-list-md>
                    <v-layout wrap>
                      <v-flex xs12>
                        <v-text-field
                          v-model="unitDesired.DesiredBrandModel"
                          value=""
                          label="Brand Model*"
                          hide-details
                          required
                          :disabled="!isEdit"
                        ></v-text-field>
                        <span class="v-input error--text v-messages">
                          <span v-if="hasUnitDesiredBrand">
                            Please enter Brand Model
                          </span>
                        </span>
                      </v-flex>
                      <v-flex xs12 sm6>
                        <v-text-field
                          v-model="unitDesired.DesiredSerialNo"
                          value=""
                          label="Serial No.*"
                          hide-details
                          required
                          :disabled="!isEdit"
                        ></v-text-field>
                        <span class="v-input error--text v-messages">
                          <span v-if="hasUnitDesiredSerial">
                            Please enter Serial No.
                          </span>
                        </span>
                      </v-flex>
                      <v-flex xs12 sm6>
                        <v-text-field
                          v-model="unitDesired.DesiredCode"
                          value=""
                          label="Code*"
                          hide-details
                          required
                          :disabled="!isEdit"
                        ></v-text-field>
                        <span class="v-input error--text v-messages">
                          <span v-if="hasUnitDesiredCode">
                            Please enter Code
                          </span>
                        </span>
                      </v-flex>
                      <v-flex xs12 sm6>
                        <v-text-field
                          v-model="unitDesired.DesiredAmount"
                          value=""
                          label="Amount*"
                          hide-details
                          required
                          :disabled="!isEdit"
                        ></v-text-field>
                        <span class="v-input error--text v-messages">
                          <span v-if="hasUnitDesiredAmount">
                            Please enter Amount
                          </span>
                        </span>
                      </v-flex>
                      <v-flex xs12 sm6>
                        <v-text-field
                          v-model="unitDesired.DesiredAccounting"
                          value=""
                          label="Accounting*"
                          hide-details
                          required
                          :disabled="!isEdit"
                        ></v-text-field>
                        <span class="v-input error--text v-messages">
                          <span v-if="hasUnitDesiredAccounting">
                            Please enter Accounting
                          </span>
                        </span>
                      </v-flex>
                    </v-layout>
                  </v-container>
                  <small>*Indicates required field</small>
                  <div><small>Please put ( - ) if not applicable</small></div>
                </v-card-text>
                <v-card-actions>
                  <v-spacer></v-spacer>
                  <v-btn
                    color="grey darken-1"
                    flat
                    @click="toggleUnitDesiredModal"
                    >Close</v-btn
                  >
                  <v-btn
                    v-if="isEditUnitDesired"
                    color="blue darken-1"
                    flat
                    @click="saveUnitDesired(indexEditUnitDesired, unitDesired)"
                    >Save</v-btn
                  >
                  <v-btn
                    v-else
                    color="blue darken-1"
                    flat
                    @click="addUnitDesired(unitDesired)"
                    >Add</v-btn
                  >
                </v-card-actions>
              </v-card>
            </v-dialog>
          </tbody>
        </table>
      </v-flex>
      <span
        class="v-input error--text v-messages align-self-center"
        v-if="hasUnitDesired"
      >
        Please add atleast 1 Unit
      </span>
      <v-flex align-self-end>
        <v-btn
          @click="toggleUnitDesiredModal"
          class="btn_primary"
          :disabled="isAddUnitDesired || !isEdit"
        >
          <v-icon>add</v-icon>
          Add Unit(s) Desired
        </v-btn>
      </v-flex>
    </v-layout>
    <!-- Unit Desired TC -->
    <v-layout mt-5 row class="title-bg">
      <v-card flat>
        <h2 class="title-header">Unit(s) Desired Terms & Conditions</h2>
      </v-card>
    </v-layout>
    <v-layout row>
      <v-divider></v-divider>
    </v-layout>
    <v-layout wrap>
      <v-flex xs12 sm6 md4 lg2 pa-2>
        <v-text-field
          v-model="unitDesiredTC.DesiredTCTerms"
          :rules="rUnitDesiredTC.DesiredTCTerms"
          value=""
          outline
          label="Terms*"
          required
          :disabled="!isEdit"
        ></v-text-field>
      </v-flex>
      <v-flex xs12 sm6 md4 lg2 pa-2>
        <v-text-field
          v-model="unitDesiredTC.DesiredTCDownPayment"
          :rules="rUnitDesiredTC.DesiredTCDownPayment"
          value=""
          outline
          label="Down Payment*"
          required
          :disabled="!isEdit"
        ></v-text-field>
      </v-flex>
      <v-flex xs12 sm6 md4 lg2 pa-2>
        <v-text-field
          v-model="unitDesiredTC.DesiredTCMonthlyInstallment"
          :rules="rUnitDesiredTC.DesiredTCMonthlyInstallment"
          value=""
          outline
          label="Monthly Installment*"
          required
          :disabled="!isEdit"
        ></v-text-field>
      </v-flex>
      <v-flex xs12 sm6 md4 lg2 pa-2>
        <v-text-field
          v-model="unitDesiredTC.DesiredTCTotalPrice"
          :rules="rUnitDesiredTC.DesiredTCTotalPrice"
          value=""
          outline
          label="Total Price*"
          required
          :disabled="!isEdit"
        ></v-text-field>
      </v-flex>
      <v-flex xs12 sm6 md4 lg2 pa-2>
        <v-text-field
          v-model="unitDesiredTC.DesiredTCTotalRebate"
          :rules="rUnitDesiredTC.DesiredTCTotalRebate"
          value=""
          outline
          label="Total Rebate*"
          required
          :disabled="!isEdit"
        ></v-text-field>
      </v-flex>
      <v-flex xs12 sm6 md4 lg2 pa-2>
        <v-text-field
          v-model="unitDesiredTC.DesiredTCRemarks"
          :rules="rUnitDesiredTC.DesiredTCRemarks"
          value=""
          outline
          label="Remarks*"
          required
          :disabled="!isEdit"
        ></v-text-field>
      </v-flex>
    </v-layout>
    <!-- <v-layout column mt-3>
      <v-flex xs12 class="table-responsive">
        <table>
          <thead>
            <tr>
              <th width="15%">Brand Model</th>
              <th width="10%">Terms</th>
              <th width="15%">Down Payment</th>
              <th width="10%">Monthly Installment</th>
              <th width="15%">Total Price</th>
              <th width="10%">Total Rebate</th>
              <th width="15%">Remarks</th>
              <th width="10%">Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(desiredTC, index) in listUnitDesiredTC" :key="index">
              <td width="15%">
                {{ desiredTC.DesiredTCBrandModel }}
              </td>
              <td width="10%">
                {{ desiredTC.DesiredTCTerms }}
              </td>
              <td width="15%">
                {{ desiredTC.DesiredTCDownPayment }}
              </td>
              <td width="10%">
                {{ desiredTC.DesiredTCMonthlyInstallment }}
              </td>
              <td width="15%">
                {{ desiredTC.DesiredTCTotalPrice }}
              </td>
              <td width="10%">
                {{ desiredTC.DesiredTCTotalRebate }}
              </td>
              <td width="15%">
                {{ desiredTC.DesiredTCRemarks }}
              </td>
              <td width="10%">
                <v-layout justify-center align-center>
                  <v-btn
                    @click="delUnitDesiredTCRow(index)"
                    class="mx-2 actions-question btn_primary"
                    fab
                    small
                    :disabled="isAddUnitDesiredTC || !isEdit"
                  >
                    <v-icon>delete</v-icon>
                  </v-btn>
                  <v-btn
                    @click="editUnitDesiredTC(index, desiredTC)"
                    class="mx-2 actions-question"
                    fab
                    small
                    color="#1332A8"
                    :disabled="isAddUnitDesiredTC || !isEdit"
                  >
                    <v-icon class="check-icon">edit</v-icon>
                  </v-btn>
                </v-layout>
              </td>
            </tr>
            <v-dialog v-model="isAddUnitDesiredTC" persistent max-width="600px">
              <v-card>
                <v-card-title
                  class="font-weight-medium grey lighten-2"
                  primary-title
                >
                  Add Unit Desired Terms & Conditions
                </v-card-title>
                <v-card-text>
                  <v-container grid-list-md>
                    <v-layout wrap>
                      <v-flex xs12>
                        <v-text-field
                          v-model="unitDesiredTC.DesiredTCBrandModel"
                          value=""
                          label="Brand Model*"
                          hide-details
                          required
                          :disabled="!isEdit"
                        ></v-text-field>
                        <span class="v-input error--text v-messages">
                          <span v-if="hasUnitDesiredTCBrand">
                            Please enter Brand Model
                          </span>
                        </span>
                      </v-flex>
                      <v-flex xs12 sm6>
                        <v-text-field
                          v-model="unitDesiredTC.DesiredTCTerms"
                          value=""
                          label="Terms*"
                          hide-details
                          required
                          :disabled="!isEdit"
                        ></v-text-field>
                        <span class="v-input error--text v-messages">
                          <span v-if="hasUnitDesiredTCTerms">
                            Please enter Terms
                          </span>
                        </span>
                      </v-flex>
                      <v-flex xs12 sm6>
                        <v-text-field
                          v-model="unitDesiredTC.DesiredTCDownPayment"
                          value=""
                          label="Down Payment*"
                          hide-details
                          required
                          :disabled="!isEdit"
                        ></v-text-field>
                        <span class="v-input error--text v-messages">
                          <span v-if="hasUnitDesiredTCDownPayment">
                            Please enter Down Payment
                          </span>
                        </span>
                      </v-flex>
                      <v-flex xs12 sm6>
                        <v-text-field
                          v-model="unitDesiredTC.DesiredTCMonthlyInstallment"
                          value=""
                          label="Monthly Installment*"
                          hide-details
                          required
                          :disabled="!isEdit"
                        ></v-text-field>
                        <span class="v-input error--text v-messages">
                          <span v-if="hasUnitDesiredTCMonthlyInstallment">
                            Please enter Monthly Installment
                          </span>
                        </span>
                      </v-flex>
                      <v-flex xs12 sm6>
                        <v-text-field
                          v-model="unitDesiredTC.DesiredTCTotalPrice"
                          value=""
                          label="Total Price*"
                          hide-details
                          required
                          :disabled="!isEdit"
                        ></v-text-field>
                        <span class="v-input error--text v-messages">
                          <span v-if="hasUnitDesiredTCTotalPrice">
                            Please enter Total Price
                          </span>
                        </span>
                      </v-flex>
                      <v-flex xs12 sm6>
                        <v-text-field
                          v-model="unitDesiredTC.DesiredTCTotalRebate"
                          value=""
                          label="Total Rebate*"
                          hide-details
                          required
                          :disabled="!isEdit"
                        ></v-text-field>
                        <span class="v-input error--text v-messages">
                          <span v-if="hasUnitDesiredTCTotalRebate">
                            Please enter Total Rebate
                          </span>
                        </span>
                      </v-flex>
                      <v-flex xs12 sm6>
                        <v-text-field
                          v-model="unitDesiredTC.DesiredTCRemarks"
                          value=""
                          label="Remarks*"
                          hide-details
                          required
                          :disabled="!isEdit"
                        ></v-text-field>
                        <span class="v-input error--text v-messages">
                          <span v-if="hasUnitDesiredTCRemarks">
                            Please enter Remarks
                          </span>
                        </span>
                      </v-flex>
                    </v-layout>
                  </v-container>
                  <small>*Indicates required field</small>
                  <div><small>Please put ( - ) if not applicable</small></div>
                </v-card-text>
                <v-card-actions>
                  <v-spacer></v-spacer>
                  <v-btn
                    color="grey darken-1"
                    flat
                    @click="toggleUnitDesiredTCModal"
                    >Close</v-btn
                  >
                  <v-btn
                    v-if="isEditUnitDesiredTC"
                    color="blue darken-1"
                    flat
                    @click="
                      saveUnitDesiredTC(indexEditUnitDesiredTC, unitDesiredTC)
                    "
                    >Save</v-btn
                  >
                  <v-btn
                    v-else
                    color="blue darken-1"
                    flat
                    @click="addUnitDesiredTC(unitDesiredTC)"
                    >Add</v-btn
                  >
                </v-card-actions>
              </v-card>
            </v-dialog>
          </tbody>
        </table>
      </v-flex>
      <span
        class="v-input error--text v-messages align-self-center"
        v-if="hasUnitDesiredTC"
      >
        Please add atleast 1 Terms & Conditions
      </span>
      <v-flex align-self-end>
        <v-btn
          @click="toggleUnitDesiredTCModal"
          class="btn_primary"
          :disabled="isAddUnitDesiredTC || !isEdit"
        >
          <v-icon>add</v-icon>
          Add Terms and Condition
        </v-btn>
      </v-flex>
    </v-layout> -->
    <v-layout>
      <v-flex xs12 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Attachment <b class="red--text">*</b></b>
            <i>(Attach up to 5 files only)</i>
          </v-card-title>
          <v-container class="mt-4">
            <v-layout column wrap>
              <v-flex xs12 align-self-start>
                <span class="v-input error--text v-messages">
                  <span v-if="!hasFileAttachment">
                    Please attach atleast 1 file
                  </span>
                </span>
              </v-flex>
              <v-layout column align-center>
                <v-layout
                  v-for="(attachment, index) in this.mainAttachments"
                  :key="index"
                  justify-space-between
                  align-center
                  class="break-word attachment-content"
                >
                  <u
                    ><v-icon>attach_file</v-icon
                    ><a
                      @click="
                        download(
                          attachment.LoanID,
                          attachment.FileName,
                          'Attachments'
                        )
                      "
                      ><strong>{{ attachment.FileName }}</strong></a
                    ></u
                  >
                  <v-btn
                    color="red"
                    flat
                    icon
                    @click="DeleteAttachment(index)"
                    :disabled="!isEdit"
                  >
                    <v-icon>delete</v-icon>
                  </v-btn>
                </v-layout>
              </v-layout>
              <v-layout justify-space-around align-center v-if="isEdit">
                <label append-outer-icon="place" for="up" class="btn-upload">
                  <v-icon>attach_file</v-icon>
                  Add attachment
                </label>
                <input
                  id="up"
                  type="file"
                  multiple
                  @change.prevent="listUploads"
                  accept="*"
                  class="d-none"
                  ref="file-input"
                  required
                  :disabled="!isEdit"
                />
              </v-layout>
            </v-layout>
            <v-layout column align-center v-show="showAttachments">
              <v-layout
                v-for="(attachment, index) in displayAttachments"
                :key="index"
                justify-space-between
                align-center
                class="break-word attachment-content"
              >
                <div>
                  <strong>{{ attachment.name }}</strong>
                  <div>{{ attachment.size | formatBytes }}</div>
                </div>

                <v-btn color="red" flat icon @click="RemoveAttachment(index)">
                  <v-icon>remove_circle_outline</v-icon>
                </v-btn>
              </v-layout>
              <v-flex xs12>
                <v-btn @click.prevent="ResetFileField" class="btn_secondary">
                  Clear
                </v-btn>
              </v-flex>
            </v-layout>
          </v-container>
        </v-card>
      </v-flex>
    </v-layout>
    <!-- Confirmation -->
    <v-layout mt-5 row class="title-bg">
      <v-card flat>
        <h2 class="title-header">Confirmation</h2>
      </v-card>
    </v-layout>
    <v-layout row>
      <v-divider></v-divider>
    </v-layout>
    <v-layout column>
      <v-checkbox
        v-model="confirmation.IsAgreed"
        label="I hereby affirm that each of the answer foregoing question is true and correct, and authorize you to obtain such information as you may require concerning this application and agree that such information shall remain your property whether or not this application is model.data.. "
        :rules="rConfirmation.IsAgreed"
        @click.native="toggleConfirmationModal"
        required
        disabled
        hide-details
      ></v-checkbox>
      <v-dialog v-model="confirmDialog" persistent max-width="800px">
        <v-card>
          <v-card-title class="headline">Confirmation Signature</v-card-title>
          <v-card-text>
            I hereby affirm that each of the answer foregoing question is true
            and correct, and authorize you to obtain such information as you may
            require concerning this application and agree that such information
            shall remain your property whether or not this application is
            model.data..
            <VueSignaturePad
              id="signature"
              height="400px"
              :options="{
                onBegin: () => {
                  $refs.signaturePad.resizeCanvas();
                }
              }"
              ref="signaturePad"
            />
            <span class="v-input error--text v-messages align-self-center">
              <span v-if="isEmptySignature">Please affix your signature</span>
            </span>
            <v-layout row justify-center>
              <div class="mt-2">
                <v-btn flat small @click="UndoSignature">Undo</v-btn>
              </div>
              <div class="mt-2">
                <v-btn flat small @click="ClearSignature">Clear</v-btn>
              </div>
            </v-layout>
          </v-card-text>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="grey darken-1" flat @click="toggleConfirmationModal"
              >Disagree</v-btn
            >
            <v-btn color="primary darken-1" flat @click="SaveSignature"
              >Agree</v-btn
            >
          </v-card-actions>
        </v-card>
      </v-dialog>
    </v-layout>
    <v-layout row wrap justify-start>
      <v-flex xs12 sm6 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Closing Officer</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="confirmation.ClosingOfficer"
              :rules="rConfirmation.ClosingOfficer"
              color="purple darken-2"
              label="Closing Officer"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm3 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Date</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-menu
              transition="scale-transition"
              :close-on-content-click="false"
              lazy
              offset-y
              full-width
            >
              <template v-slot:activator="{ on }">
                <v-text-field
                  v-model="confirmation.Date"
                  v-on="on"
                  append-icon="event"
                  readonly
                  placeholder="YYYY-MM-DD"
                  color="red"
                  class="text-center"
                  single-line
                  outline
                  :disabled="!isEdit"
                >
                </v-text-field>
              </template>
              <v-date-picker
                v-model="confirmation.Date"
                class="customTable"
                color="red"
              >
              </v-date-picker>
            </v-menu>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm3 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Time</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-menu
              transition="scale-transition"
              :close-on-content-click="false"
              lazy
              offset-y
              full-width
            >
              <template v-slot:activator="{ on }">
                <v-text-field
                  v-model="confirmation.Time"
                  v-on="on"
                  append-icon="schedule"
                  readonly
                  placeholder="H:m"
                  color="red"
                  class="text-center"
                  single-line
                  outline
                  :disabled="!isEdit"
                >
                </v-text-field>
              </template>
              <v-time-picker
                v-model="confirmation.Time"
                class="customTable"
                color="red"
              ></v-time-picker>
            </v-menu>
          </v-container>
        </v-card>
      </v-flex>
    </v-layout>
    <!-- title - For Office Use Only -->
    <v-layout mt-5 row class="title-bg">
      <v-card flat>
        <h2 class="title-header">For Office Use Only</h2>
      </v-card>
    </v-layout>
    <v-layout row>
      <v-divider></v-divider>
    </v-layout>
    <v-layout row wrap justify-start>
      <v-flex xs12 sm6 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Credit Analyst Name</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="officeUse.CAName"
              :rules="rOfficeUse.CAName"
              color="purple darken-2"
              label="Credit Analyst Name"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Remarks</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="officeUse.CARemarks"
              :rules="rOfficeUse.CARemarks"
              color="purple darken-2"
              label="Remarks"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Date</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-menu
              transition="scale-transition"
              :close-on-content-click="false"
              lazy
              offset-y
              full-width
            >
              <template v-slot:activator="{ on }">
                <v-text-field
                  v-model="officeUse.CADate"
                  v-on="on"
                  append-icon="event"
                  readonly
                  placeholder="YYYY-MM-DD"
                  color="red"
                  class="text-center"
                  single-line
                  outline
                  :disabled="!isEdit"
                >
                </v-text-field>
              </template>
              <v-date-picker
                v-model="officeUse.CADate"
                class="customTable"
                color="red"
              >
              </v-date-picker>
            </v-menu>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Time</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-menu
              transition="scale-transition"
              :close-on-content-click="false"
              lazy
              offset-y
              full-width
            >
              <template v-slot:activator="{ on }">
                <v-text-field
                  v-model="officeUse.CATime"
                  v-on="on"
                  append-icon="schedule"
                  readonly
                  placeholder="H:m"
                  color="red"
                  class="text-center"
                  single-line
                  outline
                  :disabled="!isEdit"
                >
                </v-text-field>
              </template>
              <v-time-picker
                v-model="officeUse.CATime"
                class="customTable"
                color="red"
              ></v-time-picker>
            </v-menu>
          </v-container>
        </v-card>
      </v-flex>
    </v-layout>
    <v-layout row>
      <v-divider></v-divider>
    </v-layout>
    <v-layout row wrap justify-start>
      <v-flex xs12 sm6 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">CCS Name</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="officeUse.CCSName"
              :rules="rOfficeUse.CCSName"
              color="purple darken-2"
              label="CCS Name"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Remarks</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="officeUse.CCSRemarks"
              :rules="rOfficeUse.CCSRemarks"
              color="purple darken-2"
              label="Remarks"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Date</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-menu
              transition="scale-transition"
              :close-on-content-click="false"
              lazy
              offset-y
              full-width
            >
              <template v-slot:activator="{ on }">
                <v-text-field
                  v-model="officeUse.CCSDate"
                  v-on="on"
                  append-icon="event"
                  readonly
                  placeholder="YYYY-MM-DD"
                  color="red"
                  class="text-center"
                  single-line
                  outline
                  :disabled="!isEdit"
                >
                </v-text-field>
              </template>
              <v-date-picker
                v-model="officeUse.CCSDate"
                class="customTable"
                color="red"
              >
              </v-date-picker>
            </v-menu>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Time</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-menu
              transition="scale-transition"
              :close-on-content-click="false"
              lazy
              offset-y
              full-width
            >
              <template v-slot:activator="{ on }">
                <v-text-field
                  v-model="officeUse.CCSTime"
                  v-on="on"
                  append-icon="schedule"
                  readonly
                  placeholder="H:m"
                  color="red"
                  class="text-center"
                  single-line
                  outline
                  :disabled="!isEdit"
                >
                </v-text-field>
              </template>
              <v-time-picker
                v-model="officeUse.CCSTime"
                class="customTable"
                color="red"
              ></v-time-picker>
            </v-menu>
          </v-container>
        </v-card>
      </v-flex>
    </v-layout>
    <v-layout row>
      <v-divider></v-divider>
    </v-layout>
    <v-layout row wrap justify-start>
      <v-flex xs12 sm6 md3 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Charge Invoice Number</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="officeUse.ChargeInvoiceNumber"
              :rules="rOfficeUse.ChargeInvoiceNumber"
              color="purple darken-2"
              label="Charge Invoice Number"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md3 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Invoice Date</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-menu
              transition="scale-transition"
              :close-on-content-click="false"
              lazy
              offset-y
              full-width
            >
              <template v-slot:activator="{ on }">
                <v-text-field
                  v-model="officeUse.InvoiceDate"
                  v-on="on"
                  append-icon="event"
                  readonly
                  placeholder="YYYY-MM-DD"
                  color="red"
                  class="text-center"
                  single-line
                  outline
                  :disabled="!isEdit"
                >
                </v-text-field>
              </template>
              <v-date-picker
                v-model="officeUse.InvoiceDate"
                class="customTable"
                color="red"
              >
              </v-date-picker>
            </v-menu>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md3 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">O.R. Number</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="officeUse.ORNumber"
              :rules="rOfficeUse.ORNumber"
              color="purple darken-2"
              label="O.R. Number"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md3 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">O.R. Date</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-menu
              transition="scale-transition"
              :close-on-content-click="false"
              lazy
              offset-y
              full-width
            >
              <template v-slot:activator="{ on }">
                <v-text-field
                  v-model="officeUse.ORDate"
                  v-on="on"
                  append-icon="event"
                  readonly
                  placeholder="YYYY-MM-DD"
                  color="red"
                  class="text-center"
                  single-line
                  outline
                  :disabled="!isEdit"
                >
                </v-text-field>
              </template>
              <v-date-picker
                v-model="officeUse.ORDate"
                class="customTable"
                color="red"
              >
              </v-date-picker>
            </v-menu>
          </v-container>
        </v-card>
      </v-flex>
    </v-layout>
    <v-layout column wrap align-end justify-end>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Amount</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="officeUse.Amount"
              :rules="rOfficeUse.Amount"
              color="purple darken-2"
              label="Amount"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
      <v-flex xs12 sm6 md4 pa-2>
        <v-card class="info-card">
          <v-card-title class="grey lighten-2 index-label">
            <b class="body-2">Cashier</b>
          </v-card-title>
          <v-container class="mt-4">
            <v-text-field
              v-model="officeUse.Cashier"
              :rules="rOfficeUse.Cashier"
              color="purple darken-2"
              label="Cashier"
              single-line
              outline
              clearable
              :disabled="!isEdit"
            ></v-text-field>
          </v-container>
        </v-card>
      </v-flex>
    </v-layout>
  </v-form>
</template>
<script>
import { mapGetters } from "vuex";
import offline from "v-offline";
import confirm from "../../../common/layout/confirm-modal";
import info from "../../../common/layout/info-modal";
import loading from "../../../common/layout/progress";
import validators from "@/common/utils/form/validators";
import constants from "../../../common/utils/constants";
import moment from "moment";
export default {
  data() {
    return {
      userDefaultRegion: "",
      userDefaultCity: "",
      isEdit: false,

      valid: false,
      status: true,
      fullscreenLoading: false,

      chooseBirthDate: false,

      //COMMON
      validateNumber: [
        v => !v || /^[0-9]+$/.test(v) || "Please input a valid number"
      ],

      //CheckBox Source of Income
      cbSOI: [],

      //CheckBox Skip section
      skipChildren: false,
      skipParentLaw: true,
      skipSpouse: true,

      selectHomeAddress: false,
      //PERSONAL INFO
      personalInfo: {
        SelectedBranch: null,
        Name: "",
        BirthDate: "",
        Age: 0,
        HomeAddress: "",
        Region: {
          text: "",
          value: "",
          code: null
        },
        City: {
          text: "",
          value: "",
          code: ""
        },
        HouseUnitBuildingNo: "",
        StreetBarangay: "",
        Landmark: "",
        BirthPlace: "",
        PreviousAddress: "",
        Phone: "",
        Telephone: "",
        Email: "",
        Facebook: "",
        RentingName: "",
        RentingAddress: "",
        RentingTelNo: "",
        Residence: "",
        Status: "",
        Land: "",
        HouseMade: ""
      },
      rPersonalInfo: {
        SelectedBranch: [v => !!v || "Branch is required"],
        Name: [v => !!v || "Name is required"],
        BirthDate: [v => !!v || "Birth date is required"],
        BirthPlace: [v => !!v || "Birth place is required"],
        HomeAddress: [v => !!v || "Present Home Address is required"],
        PreviousAddress: [v => !!v || "Previous Address is required"],
        Phone: [
          v => !!v || "Cellphone Number is required",
          v => !v || /^[0-9]+$/.test(v) || "Please input a valid number"
        ],
        Email: [
          v => !!v || "Email is required",
          v =>
            !v ||
            /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/.test(v) ||
            "E-mail must be valid"
        ],
        RentingName: [],
        RentingAddress: [],
        Status: [v => !!v || "Status is required"],
        Residence: [v => !!v || "Stability of Residence is required"],
        Land: [v => !!v || "Land is required"],
        HouseMade: [v => !!v || "House made is required"]
      },

      //SOURCE OF INCOME
      industry: "",
      rIndustry: [v => !!v || "Industry is required"],
      employed: {
        TypeOf: "",
        Present: "",
        HowLong: "",
        Position: "",
        PresentAddress: "",
        Telephone: "",
        Salary: "",
        Previous: "",
        PreviousAddress: ""
      },
      rEmployed: {
        TypeOf: [v => !!v || "Type is required"],
        Present: [v => !!v || "Present Employment is required"],
        PresentAddress: [v => !!v || "Present business address is required"],
        HowLong: [v => !!v || "How Long is required"],
        Position: [v => !!v || "Position is required"],
        Salary: [v => !!v || "Monthly Salary is required"]
      },

      selfOwn: {
        BusinessNature: "",
        BusinessName: "",
        BusinessAddress: "",
        Capital: "",
        Net: "",
        Gross: "",
        PhoneNumber: "",
        HowLong: ""
      },
      rSelfOwn: {
        BusinessNature: [v => !!v || "Nature of Business is required"],
        BusinessName: [v => !!v || "Business Name is required"],
        BusinessAddress: [v => !!v || "Business Address is required"]
      },

      pension: {
        Agency: "",
        Monthly: ""
      },
      rPension: {
        Agency: [v => !!v || "Agency of Pension is required"],
        Monthly: [v => !!v || "Monthly Pension is required"]
      },

      remittance: {
        Name: "",
        Location: "",
        Relationship: "",
        MonthlyAmount: "",
        Frequency: ""
      },
      rRemittance: {
        Name: [v => !!v || "Name of Remitter is required"],
        Location: [v => !!v || "Location of Remitter is required"],
        Relationship: [v => !!v || "Relationship of Remitter is required"],
        MonthlyAmount: [v => !!v || "Monthly Remittance Amount is required"],
        Frequency: [v => !!v || "Frequency of Remittance is required"]
      },

      selectionIndustry: [
        "Agriculture, Aquaculture, Forestry & Animal Production",
        "Animal Production",
        "Mining Quarrying",
        "Manufacturing",
        "Electricity, Gas Steam & Air Conditioning Supply",
        "Water Supply, Sewerage, Waste Management",
        "Construction",
        "Wholesale & Retail Trade, Repair of Machineries",
        "Transportation & Storage",
        "Accomodation & Food Service Activities",
        "Information & Communication",
        "Financial & Insurance Activities",
        "Real Estate Activities",
        "Professional, Scientific & Technical Activities",
        "Administrative & Support Service Activities",
        "Public Administrative Defense, Compulsary Social Security",
        "Education",
        "Human Health & Social Work Activities",
        "Arts, Entertainment & Recreation",
        "Other Service Activities",
        "Household Activities",
        "Activities of Extraterritorial Organizations & Bodies"
      ],
      selectionBusinessNature: [
        "Sole Proprietorship",
        "Partnership",
        "Corporation",
        "Cooperative"
      ],
      selectionRelationship: [
        "Spouse",
        "Child",
        "Parent",
        "Relative",
        "Friend"
      ],
      selectionFrequency: [
        " Daily",
        "Monthly",
        "Quarterly",
        "Semi-Annually",
        "Annually"
      ],

      parents: {
        Father: {
          Name: "",
          Age: "",
          Address: "",
          Facebook: "",
          Employment: "",
          OfficeAddress: "",
          Position: "",
          HowLong: ""
        },
        Mother: {
          Name: "",
          Age: "",
          Address: "",
          Facebook: "",
          Employment: "",
          OfficeAddress: "",
          Position: "",
          HowLong: ""
        }
      },
      rParents: {
        Father: {
          Name: [v => !!v || "Father's Name is required"],
          Age: [
            v => !!v || "Age is required",
            v => !v || /^[0-9]+$/.test(v) || "Please input a valid age"
          ],
          Address: [v => !!v || "Address is required"]
        },
        Mother: {
          Name: [v => !!v || "Mother's Maiden Name is required"],
          Age: [
            v => !!v || "Age is required",
            v => !v || /^[0-9]+$/.test(v) || "Please input a valid age"
          ],
          Address: [v => !!v || "Address is required"]
        }
      },

      spouse: {
        Name: "",
        Age: "",
        IncomeSource: "",
        OfficeAddress: "",
        Position: "",
        HowLong: "",
        OfficeTelNo: "",
        Salary: ""
      },
      rSpouse: {
        Name: [],
        Age: [v => !v || /^[0-9]+$/.test(v) || "Please input a valid age"]
      },

      parentsInLaw: {
        Father: {
          Name: "",
          Age: "",
          Address: "",
          Facebook: "",
          Employment: "",
          OfficeAddress: "",
          Position: "",
          HowLong: ""
        },
        Mother: {
          Name: "",
          Age: "",
          Address: "",
          Facebook: "",
          Employment: "",
          OfficeAddress: "",
          Position: "",
          HowLong: ""
        }
      },
      rParentsInLaw: {
        Father: {
          Name: [],
          Age: [v => !v || /^[0-9]+$/.test(v) || "Please input a valid age"],
          Address: []
        },
        Mother: {
          Name: [],
          Age: [v => !v || /^[0-9]+$/.test(v) || "Please input a valid age"],
          Address: []
        }
      },

      listChildren: [],
      indexEditChildren: null,
      isEditChildren: false,
      isAddChildren: false,
      children: {
        child_name: "",
        child_age: "",
        child_address: "",
        child_tel_no: "",
        child_employ_school: "",
        child_employ_school_address: "",
        child_pos_grade: "",
        child_how_long: ""
      },

      listCreditHistory: [],
      indexEditCreditHistory: null,
      isEditCreditHistory: false,
      isAddCreditHistory: false,
      creditHistory: {
        HistoryCompanyName: "",
        HistoryTypeOfUnit: "",
        HistoryDatePurchase: new Date().toISOString().substr(0, 10),
        HistoryTerms: "",
        HistoryRemainingBalance: ""
      },

      listPropertyOwned: [],
      indexEditProperty: null,
      isEditProperty: false,
      isAddProperty: false,
      property: {
        Property: ""
      },

      listUnitDesired: [],
      indexEditUnitDesired: null,
      isEditUnitDesired: false,
      isAddUnitDesired: false,
      unitDesired: {
        DesiredBrandModel: "",
        DesiredSerialNo: "",
        DesiredCode: "",
        DesiredAmount: "",
        DesiredAccounting: ""
      },

      // listUnitDesiredTC: [],
      // indexEditUnitDesiredTC: null,
      // isEditUnitDesiredTC: false,
      // isAddUnitDesiredTC: false,
      unitDesiredTC: {
        DesiredTCTerms: "",
        DesiredTCDownPayment: "",
        DesiredTCMonthlyInstallment: "",
        DesiredTCTotalPrice: "",
        DesiredTCTotalRebate: "",
        DesiredTCRemarks: ""
      },
      rUnitDesiredTC: {
        DesiredTCTerms: [v => !!v || "Terms is Required"],
        DesiredTCDownPayment: [v => !!v || "Down Payment is Required"],
        DesiredTCMonthlyInstallment: [
          v => !!v || "Monthly Installment is Required"
        ],
        DesiredTCTotalPrice: [v => !!v || "Total Price is Required"],
        DesiredTCTotalRebate: [v => !!v || "Total Rebate is Required"],
        DesiredTCRemarks: [v => !!v || "Remarks is Required"]
      },

      countAttach: 0,
      removeAttachments: [],
      newAttachments: [],
      mainAttachments: [],
      displayAttachments: [],
      showAttachments: false,

      confirmDialog: false,
      isEmptySignature: true,
      signature: {
        FileName: "signature.png",
        FileDataArray: null
      },

      confirmation: {
        IsAgreed: false,
        ClosingOfficer: "",
        Date: "",
        Time: ""
        // Date: new Date().toISOString().substr(0, 10),
        // Time: new Date().getHours() + ":" + new Date().getMinutes()
      },
      rConfirmation: {
        IsAgreed: [v => !!v || "You must agree to continue!"],
        ClosingOfficer: [v => !!v || "Closing Officer is Required"]
      },

      officeUse: {
        CAName: "",
        CARemarks: "",
        CADate: "",
        CATime: "",
        CCSName: "",
        CCSRemarks: "",
        CCSDate: "",
        CCSTime: "",
        ChargeInvoiceNumber: "",
        InvoiceDate: "",
        ORNumber: "",
        ORDate: "",
        Amount: "",
        Cashier: ""
      },
      rOfficeUse: {
        CAName: [v => !!v || "Credit Analyst Name is Required"],
        CARemarks: [v => !!v || "Credit Analyst Remarks is Required"],
        CCSName: [v => !!v || "CCS Name is Required"],
        CCSRemarks: [v => !!v || "CCS Remarks is Required"],
        ChargeInvoiceNumber: [v => !!v || "Charge Invoice Number is Required"],
        ORNumber: [v => !!v || "OR Number is Required"],
        Amount: [v => !!v || "Amount is Required"],
        Cashier: [v => !!v || "Cashier is Required"]
      }
    };
  },
  components: {
    confirm,
    info,
    loading,
    offline
  },
  created() {
    this.Initialize();
  },
  props: {
    loan_id: String,
    prevPage: String
  },
  computed: {
    hasSelectSOI() {
      return [this.cbSOI.length > 0 || ""];
    },
    hasFileAttachment() {
      return this.newAttachments.length > 0 || this.mainAttachments.length
        ? true
        : false;
    },
    hasRegion() {
      return Object.keys(this.personalInfo.Region).length === 0 ? true : false;
    },
    hasCity() {
      return Object.keys(this.personalInfo.City).length === 0 ? true : false;
    },
    hasHouseUnitBuildingNo() {
      return this.personalInfo.HouseUnitBuildingNo == "" ? true : false;
    },
    hasStreetBarangay() {
      return this.personalInfo.StreetBarangay == "" ? true : false;
    },

    hasChildName() {
      return this.children.ChildName == "" ? true : false;
    },
    hasChildAge() {
      return this.children.ChildAge == "" ? true : false;
    },
    hasChildHomeAddress() {
      return this.children.ChildHomeAddress == "" ? true : false;
    },
    hasChildTelephone() {
      return this.children.ChildTelNo == "" ? true : false;
    },
    hasChildEmploymentOrSchool() {
      return this.children.ChildEmploySchool == "" ? true : false;
    },
    hasChildPositionOrGrade() {
      return this.children.ChildPosGrade == "" ? true : false;
    },
    hasChildAddress() {
      return this.children.ChildEmploySchoolAddress == "" ? true : false;
    },
    hasChildHowLong() {
      return this.children.ChildHowLong == "" ? true : false;
    },

    hasCreditHistoryName() {
      return this.creditHistory.HistoryCompanyName == "" ? true : false;
    },
    hasCreditHistoryType() {
      return this.creditHistory.HistoryTypeOfUnit == "" ? true : false;
    },
    hasCreditHistoryDate() {
      return this.creditHistory.HistoryDatePurchase == "" ? true : false;
    },
    hasCreditHistoryTerms() {
      return this.creditHistory.HistoryTerms == "" ? true : false;
    },
    hasCreditHistoryBalance() {
      return this.creditHistory.HistoryRemainingBalance == "" ? true : false;
    },

    hasPropertyName() {
      return this.property.Property == "" ? true : false;
    },

    hasUnitDesired() {
      return this.listUnitDesired.length == 0 ? true : false;
    },
    hasUnitDesiredBrand() {
      return this.unitDesired.DesiredBrandModel == "" ? true : false;
    },
    hasUnitDesiredSerial() {
      return this.unitDesired.DesiredSerialNo == "" ? true : false;
    },
    hasUnitDesiredCode() {
      return this.unitDesired.DesiredCode == "" ? true : false;
    },
    hasUnitDesiredAmount() {
      return this.unitDesired.DesiredAmount == "" ? true : false;
    },
    hasUnitDesiredAccounting() {
      return this.unitDesired.DesiredAccounting == "" ? true : false;
    },

    // hasUnitDesiredTC() {
    //   return this.listUnitDesiredTC.length == 0 ? true : false;
    // },
    // hasUnitDesiredTCBrand() {
    //   return this.unitDesiredTC.DesiredTCBrandModel == "" ? true : false;
    // },
    // hasUnitDesiredTCTerms() {
    //   return this.unitDesiredTC.DesiredTCTerms == "" ? true : false;
    // },
    // hasUnitDesiredTCDownPayment() {
    //   return this.unitDesiredTC.DesiredTCDownPayment == "" ? true : false;
    // },
    // hasUnitDesiredTCMonthlyInstallment() {
    //   return this.unitDesiredTC.DesiredTCMonthlyInstallment == ""
    //     ? true
    //     : false;
    // },
    // hasUnitDesiredTCTotalPrice() {
    //   return this.unitDesiredTC.DesiredTCTotalPrice == "" ? true : false;
    // },
    // hasUnitDesiredTCTotalRebate() {
    //   return this.unitDesiredTC.DesiredTCTotalRebate == "" ? true : false;
    // },
    // hasUnitDesiredTCRemarks() {
    //   return this.unitDesiredTC.DesiredTCRemarks == "" ? true : false;
    // },

    ...mapGetters({
      branches: "application/installment/branches",
      regions: "application/installment/regions",
      cities: "application/installment/cities",

      model: "application/installment/model",
      errorMessage: "application/installment/errorMessage",
      successMessage: "application/installment/successMessage"
    })
  },
  methods: {
    Initialize() {
      this.$store.dispatch("application/installment/view");
      if (this.prevPage === undefined) {
        localStorage.setItem("prevPage", "installmentList");
      } else {
        localStorage.setItem("prevPage", this.prevPage);
      }
      if (this.status === false) {
        this.handleConnectivityChange(this.status);
      } else {
        this.fullscreenLoading = true;

        this.$store.dispatch("application/installment/getBranches").then(() => {
          setTimeout(() => {
            if (
              this.$store.getters["application/installment/branches"] ===
              constants.noInternet
            ) {
              this.$refs.info.open(constants.warning, constants.noInternet, {
                color: constants.error_color
              });
            }
          }, 1000);
        });

        this.$store.dispatch("application/installment/getRegion").then(() => {
          setTimeout(() => {
            if (
              this.$store.getters["application/installment/regions"] ===
              constants.noInternet
            ) {
              this.$refs.info.open(constants.warning, constants.noInternet, {
                color: constants.error_color
              });
            }
          }, 1000);
        });
        this.GetLoanDetails();
      }
    },
    GetLoanDetails() {
      this.$store
      .dispatch("application/installment/getLoan", this.loan_id)
      .then(() => {
        setTimeout(() => {
          if (this.errorMessage === null) {
            //If success retrieving, initialize here whats needed in the view
            this.model.birth_date = moment(this.model.birth_date).format(
              "YYYY-MM-DD"
            );
            this.model.created_date = moment(
              this.model.created_date
            ).format("YYYY-MM-DD hh:mm A");
            this.model.updated_date = !!this.model.updated_date
              ? moment(this.model.updated_date).format("YYYY-MM-DD hh:mm A")
              : null;
            this.model.approved_date = !!this.model.approved_date
              ? moment(this.model.approved_date).format(
                  "YYYY-MM-DD hh:mm A"
                )
              : null;
            for (
              let index = 0;
              index < this.model.loan_credit_history.length;
              index++
            ) {
              this.model.loan_credit_history[
                index
              ].HistoryDatePurchase = moment(
                this.model.loan_credit_history[index].HistoryDatePurchase
              ).format("YYYY-MM-DD");
            }
            this.model.confirmation_date = !!this.model.confirmation_date
              ? moment(this.model.confirmation_date).format("YYYY-MM-DD")
              : null;
            this.model.office_use_ca_date = !!this.model.office_use_ca_date
              ? moment(this.model.office_use_ca_date).format("YYYY-MM-DD")
              : null;
            this.model.office_use_ccs_date = !!this.model
              .office_use_ccs_date
              ? moment(this.model.office_use_ccs_date).format("YYYY-MM-DD")
              : null;
            this.model.office_use_invoice_date = !!this.model
              .office_use_invoice_date
              ? moment(this.model.office_use_invoice_date).format(
                  "YYYY-MM-DD"
                )
              : null;
            this.model.office_use_or_date = !!this.model.office_use_or_date
              ? moment(this.model.office_use_or_date).format("YYYY-MM-DD")
              : null;

            if (this.model.marital_status == "Married") {
              this.skipSpouse = false;
              this.skipParentLaw = false;
            }
            if (this.model.employed_where.length > 0) {
              this.cbSOI.push("employed");
            }
            if (this.model.business_name.length > 0) {
              this.cbSOI.push("selfEmployed");
            }
            if (this.model.pension_agency.length > 0) {
              this.cbSOI.push("pension");
            }
            if (this.model.remittance_name.length > 0) {
              this.cbSOI.push("remittance");
            }

            // remittance pension ownBusiness selfEmployed employed
            for (let index = 0; index < this.regions.length; index++) {
              if (this.regions[index].value == this.model.region_id) {
                this.personalInfo.Region = this.userDefaultRegion = this.regions[
                  index
                ];
              }
            }
            this.$store
              .dispatch(
                "application/installment/getCity",
                this.personalInfo.Region.value
              )
              .then(() => {
                setTimeout(() => {
                  if (
                    this.$store.getters[
                      "application/installment/cities"
                    ] === constants.noInternet
                  ) {
                  } else {
                    for (
                      let index = 0;
                      index < this.cities.length;
                      index++
                    ) {
                      if (this.cities[index].value == this.model.city_id) {
                        this.personalInfo.City = this.userDefaultCity = this.cities[
                          index
                        ];
                      }
                    }
                    this.ResetHomeAddressField();
                  }
                  this.fullscreenLoading = false;
                }, 1000);
              });
            this.ResetAllField();
          } else {
            this.fullscreenLoading = false;
            this.$refs.info
              .open(constants.warning, this.errorMessage, {
                color: constants.error_color
              })
              .then(() => {
                var prevPage = localStorage.getItem("prevPage");
                this.$router.push({ name: prevPage });
              });
          }
        });
      }, 1000);
    },
    UpdateLoanStatus(isApproved) {
      var confirmMessage =
        isApproved == "Approved"
          ? constants.approveApplication
          : constants.denyApplication;
      this.$refs.confirm
        .open(constants.confirm, confirmMessage, {
          color: constants.modal_color
        })
        .then(confirm => {
          if (confirm) {
            var loanID = [];
            loanID.push(parseInt(this.loan_id));
            this.fullscreenLoading = true;
            this.$store
              .dispatch("application/installment/updateLoanStatus", {
                loanID,
                isApproved
              })
              .then(() => {
                setTimeout(() => {
                  if (this.errorMessage === null) {
                    this.$refs.info
                      .open(constants.message, this.successMessage, {
                        color: constants.modal_color
                      });
                  } else {
                    this.$refs.info.open(constants.warning, this.errorMessage, {
                      color: constants.error_color
                    });
                  }
                  this.Initialize();
                  this.fullscreenLoading = false;
                }, 1000);
              });
          }
        });
    },
    ResetHomeAddressField() {
      this.personalInfo.HomeAddress =
        this.personalInfo.HouseUnitBuildingNo +
        " " +
        this.personalInfo.StreetBarangay +
        " " +
        this.personalInfo.Landmark +
        ", " +
        this.personalInfo.City.code +
        " " +
        this.personalInfo.City.text +
        ", " +
        this.personalInfo.Region.text;
      this.selectHomeAddress = false;
    },
    Back() {
      if (this.status === false) {
        this.handleConnectivityChange(this.status);
      } else {
        var prevPage = localStorage.getItem("prevPage");
        this.$router.push({ name: prevPage });
      }
    },
    Edit() {
      this.isEdit = true;
    },
    Cancel() {
      this.$refs.confirm
        .open(constants.confirm, constants.cancelConfirm, {
          color: constants.modal_color
        })
        .then(confirm => {
          if (confirm) {
            this.$refs.form.resetValidation();
            this.ResetFileField();
            this.ResetAllField();
            this.ResetHomeAddressField();
            this.isEdit = false;
          }
        });
    },
    Delete() {
      if (this.status === false) {
        this.handleConnectivityChange(this.status);
      } else {
        this.$refs.confirm
          .open(constants.confirm, constants.deleteConfirm, {
            color: constants.modal_color
          })
          .then(confirm => {
            if (confirm) {
              this.fullscreenLoading = true;
              this.$store
                .dispatch("application/installment/deleteLoan", this.loan_id)
                .then(() => {
                  setTimeout(() => {
                    this.$refs.info
                      .open(constants.message, constants.saved, {
                        color: constants.modal_color
                      })
                      .then(() => {
                        this.$router.push({ name: "installmentList" });
                      });
                    this.fullscreenLoading = false;
                  }, 1000);
                });
            }
          });
      }
    },
    Save() {
      this.$refs.form.validate();
      if (this.valid && this.hasFileAttachment) {
        if (this.status === false) {
          this.handleConnectivityChange(this.status);
        } else {
          this.$refs.confirm
            .open(constants.confirm, constants.saveConfirm, {
              color: constants.modal_color
            })
            .then(confirm => {
              if (confirm) {
                this.fullscreenLoading = true;
                this.AddToModel();

                this.$store
                  .dispatch("application/installment/updateLoan")
                  .then(() => {
                    setTimeout(() => {
                      this.$refs.info
                        .open(constants.message, constants.saved, {
                          color: constants.modal_color
                        })
                        .then(() => {
                          this.$router.push({ name: "installmentList" });
                        });
                      this.fullscreenLoading = false;
                    }, 1000);
                  });
              }
            });
        }
      } else {
        this.$nextTick(() => {
          const el = this.$el.querySelector(
            ".v-input.error--text:first-of-type"
          );
          this.$vuetify.goTo(el);
          return;
        });
      }
    },
    ResetAllField() {
      this.personalInfo.SelectedBranch = JSON.parse(
        JSON.stringify(this.model.branch_id)
      );
      this.personalInfo.Name = JSON.parse(
        JSON.stringify(this.model.client_name)
      );
      this.personalInfo.BirthDate = JSON.parse(
        JSON.stringify(this.model.birth_date)
      );
      this.personalInfo.Age = JSON.parse(JSON.stringify(this.model.age));

      this.personalInfo.Region = JSON.parse(
        JSON.stringify(this.userDefaultRegion)
      );
      this.personalInfo.City = JSON.parse(JSON.stringify(this.userDefaultCity));
      this.personalInfo.HouseUnitBuildingNo = JSON.parse(
        JSON.stringify(this.model.house_unit_building_no)
      );
      this.personalInfo.StreetBarangay = JSON.parse(
        JSON.stringify(this.model.street_barangay)
      );
      this.personalInfo.Landmark = JSON.parse(
        JSON.stringify(this.model.landmark)
      );

      this.personalInfo.BirthPlace = JSON.parse(
        JSON.stringify(this.model.birth_place)
      );
      this.personalInfo.PreviousAddress = JSON.parse(
        JSON.stringify(this.model.previous_address)
      );
      this.personalInfo.Phone = JSON.parse(JSON.stringify(this.model.phone_no));
      this.personalInfo.Telephone = JSON.parse(
        JSON.stringify(this.model.tel_no)
      );
      this.personalInfo.Email = JSON.parse(
        JSON.stringify(this.model.email_address)
      );
      this.personalInfo.Facebook = JSON.parse(
        JSON.stringify(this.model.facebook)
      );
      this.personalInfo.RentingName = JSON.parse(
        JSON.stringify(this.model.renting_name)
      );
      this.personalInfo.RentingAddress = JSON.parse(
        JSON.stringify(this.model.renting_address)
      );
      this.personalInfo.RentingTelNo = JSON.parse(
        JSON.stringify(this.model.renting_tel_no)
      );
      this.personalInfo.Residence = JSON.parse(
        JSON.stringify(this.model.stability_of_residence)
      );
      this.personalInfo.Status = JSON.parse(
        JSON.stringify(this.model.marital_status)
      );
      this.personalInfo.Land = JSON.parse(JSON.stringify(this.model.land));
      this.personalInfo.HouseMade = JSON.parse(
        JSON.stringify(this.model.house_made)
      );

      this.industry = JSON.parse(JSON.stringify(this.model.industry));
      this.employed.TypeOf = JSON.parse(JSON.stringify(this.model.type_of));

      this.employed.Present = JSON.parse(
        JSON.stringify(this.model.employed_where)
      );
      this.employed.HowLong = JSON.parse(
        JSON.stringify(this.model.employed_how_long)
      );
      this.employed.Position = JSON.parse(
        JSON.stringify(this.model.employed_position)
      );
      this.employed.PresentAddress = JSON.parse(
        JSON.stringify(this.model.employed_present_business_address)
      );
      this.employed.Telephone = JSON.parse(
        JSON.stringify(this.model.employed_tel_no)
      );
      this.employed.Salary = JSON.parse(
        JSON.stringify(this.model.employed_salary)
      );
      this.employed.Previous = JSON.parse(
        JSON.stringify(this.model.employed_previous)
      );
      this.employed.PreviousAddress = JSON.parse(
        JSON.stringify(this.model.employed_previous_business_address)
      );

      this.selfOwn.BusinessNature = JSON.parse(
        JSON.stringify(this.model.business_nature)
      );
      this.selfOwn.BusinessName = JSON.parse(
        JSON.stringify(this.model.business_name)
      );
      this.selfOwn.BusinessAddress = JSON.parse(
        JSON.stringify(this.model.business_address)
      );
      this.selfOwn.Capital = JSON.parse(
        JSON.stringify(this.model.business_capital)
      );
      this.selfOwn.Net = JSON.parse(
        JSON.stringify(this.model.business_monthly_income_net)
      );
      this.selfOwn.Gross = JSON.parse(
        JSON.stringify(this.model.business_monthly_income_gross)
      );
      this.selfOwn.PhoneNumber = JSON.parse(
        JSON.stringify(this.model.business_phone_no)
      );
      this.selfOwn.HowLong = JSON.parse(
        JSON.stringify(this.model.business_how_long)
      );

      this.pension.Agency = JSON.parse(
        JSON.stringify(this.model.pension_agency)
      );
      this.pension.Monthly = JSON.parse(
        JSON.stringify(this.model.pension_monthly)
      );

      this.remittance.Name = JSON.parse(
        JSON.stringify(this.model.remittance_name)
      );
      this.remittance.Location = JSON.parse(
        JSON.stringify(this.model.remittance_location)
      );
      this.remittance.Relationship = JSON.parse(
        JSON.stringify(this.model.remittance_relationship)
      );
      this.remittance.MonthlyAmount = JSON.parse(
        JSON.stringify(this.model.remittance_monthly_amount)
      );
      this.remittance.Frequency = JSON.parse(
        JSON.stringify(this.model.remittance_frequency)
      );

      this.parents.Father.Name = JSON.parse(
        JSON.stringify(this.model.father_name)
      );
      this.parents.Father.Age = JSON.parse(
        JSON.stringify(this.model.father_age)
      );
      this.parents.Father.Address = JSON.parse(
        JSON.stringify(this.model.father_address)
      );
      this.parents.Father.Facebook = JSON.parse(
        JSON.stringify(this.model.father_facebook)
      );
      this.parents.Father.Employment = JSON.parse(
        JSON.stringify(this.model.father_income_source)
      );
      this.parents.Father.OfficeAddress = JSON.parse(
        JSON.stringify(this.model.father_office_address)
      );
      this.parents.Father.Position = JSON.parse(
        JSON.stringify(this.model.father_position)
      );
      this.parents.Father.HowLong = JSON.parse(
        JSON.stringify(this.model.father_how_long)
      );

      this.parents.Mother.Name = JSON.parse(
        JSON.stringify(this.model.mother_name)
      );
      this.parents.Mother.Age = JSON.parse(
        JSON.stringify(this.model.mother_age)
      );
      this.parents.Mother.Address = JSON.parse(
        JSON.stringify(this.model.mother_address)
      );
      this.parents.Mother.Facebook = JSON.parse(
        JSON.stringify(this.model.mother_facebook)
      );
      this.parents.Mother.Employment = JSON.parse(
        JSON.stringify(this.model.mother_income_source)
      );
      this.parents.Mother.OfficeAddress = JSON.parse(
        JSON.stringify(this.model.mother_office_address)
      );
      this.parents.Mother.Position = JSON.parse(
        JSON.stringify(this.model.mother_position)
      );
      this.parents.Mother.HowLong = JSON.parse(
        JSON.stringify(this.model.mother_how_long)
      );

      this.spouse.Name = JSON.parse(JSON.stringify(this.model.spouse_name));
      this.spouse.Age = JSON.parse(JSON.stringify(this.model.spouse_age));
      this.spouse.IncomeSource = JSON.parse(
        JSON.stringify(this.model.spouse_income_source)
      );
      this.spouse.OfficeAddress = JSON.parse(
        JSON.stringify(this.model.spouse_office_address)
      );
      this.spouse.Position = JSON.parse(
        JSON.stringify(this.model.spouse_position)
      );
      this.spouse.HowLong = JSON.parse(
        JSON.stringify(this.model.spouse_how_long)
      );
      this.spouse.OfficeTelNo = JSON.parse(
        JSON.stringify(this.model.spouse_tel_no)
      );
      this.spouse.Salary = JSON.parse(JSON.stringify(this.model.spouse_salary));

      this.parentsInLaw.Father.Name = JSON.parse(
        JSON.stringify(this.model.father_in_law_name)
      );
      this.parentsInLaw.Father.Age = JSON.parse(
        JSON.stringify(this.model.father_in_law_age)
      );
      this.parentsInLaw.Father.Address = JSON.parse(
        JSON.stringify(this.model.father_in_law_address)
      );
      this.parentsInLaw.Father.Facebook = JSON.parse(
        JSON.stringify(this.model.father_in_law_facebook)
      );
      this.parentsInLaw.Father.Employment = JSON.parse(
        JSON.stringify(this.model.father_in_law_income_source)
      );
      this.parentsInLaw.Father.OfficeAddress = JSON.parse(
        JSON.stringify(this.model.father_in_law_office_address)
      );
      this.parentsInLaw.Father.Position = JSON.parse(
        JSON.stringify(this.model.father_in_law_position)
      );
      this.parentsInLaw.Father.HowLong = JSON.parse(
        JSON.stringify(this.model.father_in_law_how_long)
      );

      this.parentsInLaw.Mother.Name = JSON.parse(
        JSON.stringify(this.model.mother_in_law_name)
      );
      this.parentsInLaw.Mother.Age = JSON.parse(
        JSON.stringify(this.model.mother_in_law_age)
      );
      this.parentsInLaw.Mother.Address = JSON.parse(
        JSON.stringify(this.model.mother_in_law_address)
      );
      this.parentsInLaw.Mother.Facebook = JSON.parse(
        JSON.stringify(this.model.mother_in_law_facebook)
      );
      this.parentsInLaw.Mother.Employment = JSON.parse(
        JSON.stringify(this.model.mother_in_law_income_source)
      );
      this.parentsInLaw.Mother.OfficeAddress = JSON.parse(
        JSON.stringify(this.model.mother_in_law_office_address)
      );
      this.parentsInLaw.Mother.Position = JSON.parse(
        JSON.stringify(this.model.mother_in_law_position)
      );
      this.parentsInLaw.Mother.HowLong = JSON.parse(
        JSON.stringify(this.model.mother_in_law_how_long)
      );

      this.unitDesiredTC.DesiredTCTerms = JSON.parse(
        JSON.stringify(this.model.desired_tc_terms)
      );
      this.unitDesiredTC.DesiredTCDownPayment = JSON.parse(
        JSON.stringify(this.model.desired_tc_down_payment)
      );
      this.unitDesiredTC.DesiredTCMonthlyInstallment = JSON.parse(
        JSON.stringify(this.model.desired_tc_monthly_installment)
      );
      this.unitDesiredTC.DesiredTCTotalPrice = JSON.parse(
        JSON.stringify(this.model.desired_tc_total_price)
      );
      this.unitDesiredTC.DesiredTCTotalRebate = JSON.parse(
        JSON.stringify(this.model.desired_tc_total_rebate)
      );
      this.unitDesiredTC.DesiredTCRemarks = JSON.parse(
        JSON.stringify(this.model.desired_tc_remarks)
      );

      this.confirmation.IsAgreed = JSON.parse(
        JSON.stringify(this.model.is_agreed)
      );
      this.confirmation.ClosingOfficer = JSON.parse(
        JSON.stringify(this.model.confirmation_officer)
      );
      this.confirmation.Date = JSON.parse(
        JSON.stringify(this.model.confirmation_date)
      );
      this.confirmation.Time = JSON.parse(
        JSON.stringify(this.model.confirmation_time)
      );
      this.signature = JSON.parse(JSON.stringify(this.model.signature));

      this.officeUse.CAName = JSON.parse(
        JSON.stringify(this.model.office_use_ca_name)
      );
      this.officeUse.CARemarks = JSON.parse(
        JSON.stringify(this.model.office_use_ca_remarks)
      );
      this.officeUse.CADate = JSON.parse(
        JSON.stringify(this.model.office_use_ca_date)
      );
      this.officeUse.CATime = JSON.parse(
        JSON.stringify(this.model.office_use_ca_time)
      );

      this.officeUse.CCSName = JSON.parse(
        JSON.stringify(this.model.office_use_ccs_name)
      );
      this.officeUse.CCSRemarks = JSON.parse(
        JSON.stringify(this.model.office_use_ccs_remarks)
      );
      this.officeUse.CCSDate = JSON.parse(
        JSON.stringify(this.model.office_use_ccs_date)
      );
      this.officeUse.CCSTime = JSON.parse(
        JSON.stringify(this.model.office_use_ccs_time)
      );

      this.officeUse.ChargeInvoiceNumber = JSON.parse(
        JSON.stringify(this.model.office_use_invoice_no)
      );
      this.officeUse.InvoiceDate = JSON.parse(
        JSON.stringify(this.model.office_use_invoice_date)
      );
      this.officeUse.ORNumber = JSON.parse(
        JSON.stringify(this.model.office_use_or_no)
      );
      this.officeUse.ORDate = JSON.parse(
        JSON.stringify(this.model.office_use_or_date)
      );
      this.officeUse.Amount = JSON.parse(
        JSON.stringify(this.model.office_use_amount)
      );
      this.officeUse.Cashier = JSON.parse(
        JSON.stringify(this.model.office_use_cashier)
      );

      this.listChildren = JSON.parse(
        JSON.stringify(this.model.loan_customer_children)
      );
      this.listCreditHistory = JSON.parse(
        JSON.stringify(this.model.loan_credit_history)
      );
      this.listPropertyOwned = JSON.parse(
        JSON.stringify(this.model.loan_personal_property)
      );
      this.listUnitDesired = JSON.parse(
        JSON.stringify(this.model.loan_unit_desired)
      );
      // this.listUnitDesiredTC = JSON.parse(
      //   JSON.stringify(this.model.loan_unit_desiredtc)
      // );

      this.mainAttachments = JSON.parse(
        JSON.stringify(this.model.loan_attachment)
      );
      this.countAttach = this.mainAttachments.length;
    },
    CalculateAge() {
      var today = new Date();
      var birthDate = new Date(this.personalInfo.BirthDate);
      var age = today.getUTCFullYear() - birthDate.getUTCFullYear();
      var m = today.getMonth() - birthDate.getMonth();
      if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
        age--;
      }
      this.personalInfo.Age = age;
    },
    toggleConfirmationModal() {
      if (this.isEdit) {
        this.confirmation.IsAgreed = false;
        this.confirmDialog = !this.confirmDialog;
      }
    },
    NumbersOnly(e) {
      let keyCode = e.keyCode ? e.keyCode : e.which;
      if ((keyCode < 48 || keyCode > 57) && keyCode !== 45) {
        e.preventDefault();
      }
    },
    RegionChange(RegionID) {
      this.personalInfo.City = {};
      this.fullscreenLoading = true;
      this.$store
        .dispatch("application/installment/getCity", RegionID)
        .then(() => {
          setTimeout(() => {
            if (
              this.$store.getters["application/installment/cities"] ===
              constants.noInternet
            ) {
            }

            this.fullscreenLoading = false;
          }, 1000);
        });
    },
    ValidateHomeAddress() {
      if (
        !this.hasRegion &&
        !this.hasCity &&
        !this.hasStreetBarangay &&
        !this.hasHouseUnitBuildingNo &&
        !this.hasStreetBarangay
      ) {
        this.personalInfo.HomeAddress =
          this.personalInfo.HouseUnitBuildingNo +
          " " +
          this.personalInfo.StreetBarangay +
          ", " +
          this.personalInfo.Landmark +
          " " +
          this.personalInfo.City.code +
          " " +
          this.personalInfo.City.text +
          ", " +
          this.personalInfo.Region.text;
        this.selectHomeAddress = false;
      } else {
      }
    },
    UndoSignature() {
      this.$refs.signaturePad.undoSignature();
    },
    ClearSignature() {
      this.$refs.signaturePad.clearSignature();
    },
    SaveSignature() {
      const { isEmpty, data } = this.$refs.signaturePad.saveSignature();
      this.isEmptySignature = isEmpty;

      if (!isEmpty) {
        var BASE64_MARKER = ";base64,";
        var base64Index = data.indexOf(BASE64_MARKER) + BASE64_MARKER.length;
        var base64 = data.substring(base64Index);
        var raw = window.atob(base64);
        var rawLength = raw.length;
        var byteArray = new Array(new ArrayBuffer(rawLength));

        for (var i = 0; i < rawLength; i++) {
          byteArray[i] = raw.charCodeAt(i);
        }
        this.confirmation.IsAgreed = true;
        this.confirmDialog = false;
        this.signature.FileDataArray = byteArray;
      }
    },
    AddToModel() {
      this.$store.getters[
        constants.loanModel
      ].client_name = this.personalInfo.Name;

      this.$store.getters[
        constants.loanModel
      ].branch_id = this.personalInfo.SelectedBranch;
      this.$store.getters[
        constants.loanModel
      ].client_name = this.personalInfo.Name;
      this.$store.getters[
        constants.loanModel
      ].birth_date = this.personalInfo.BirthDate;
      this.$store.getters[constants.loanModel].age = this.personalInfo.Age;

      this.$store.getters[
        constants.loanModel
      ].region_id = this.personalInfo.Region.value;
      this.$store.getters[
        constants.loanModel
      ].city_id = this.personalInfo.City.value;
      this.$store.getters[
        constants.loanModel
      ].house_unit_building_no = this.personalInfo.HouseUnitBuildingNo;
      this.$store.getters[
        constants.loanModel
      ].street_barangay = this.personalInfo.StreetBarangay;
      this.$store.getters[
        constants.loanModel
      ].landmark = this.personalInfo.Landmark;

      this.$store.getters[
        constants.loanModel
      ].birth_place = this.personalInfo.BirthPlace;
      this.$store.getters[
        constants.loanModel
      ].previous_address = this.personalInfo.PreviousAddress;
      this.$store.getters[
        constants.loanModel
      ].phone_no = this.personalInfo.Phone;
      this.$store.getters[
        constants.loanModel
      ].tel_no = this.personalInfo.Telephone;
      this.$store.getters[
        constants.loanModel
      ].email_address = this.personalInfo.Email;
      this.$store.getters[
        constants.loanModel
      ].facebook = this.personalInfo.Facebook;
      this.$store.getters[
        constants.loanModel
      ].renting_name = this.personalInfo.RentingName;
      this.$store.getters[
        constants.loanModel
      ].renting_address = this.personalInfo.RentingAddress;
      this.$store.getters[
        constants.loanModel
      ].renting_tel_no = this.personalInfo.RentingTelNo;
      this.$store.getters[
        constants.loanModel
      ].stability_of_residence = this.personalInfo.Residence;
      this.$store.getters[
        constants.loanModel
      ].marital_status = this.personalInfo.Status;
      this.$store.getters[constants.loanModel].land = this.personalInfo.Land;
      this.$store.getters[
        constants.loanModel
      ].house_made = this.personalInfo.HouseMade;

      this.$store.getters[constants.loanModel].industry = this.industry;
      this.$store.getters[constants.loanModel].type_of = this.employed.TypeOf;

      this.$store.getters[
        constants.loanModel
      ].employed_where = this.employed.Present;
      this.$store.getters[
        constants.loanModel
      ].employed_how_long = this.employed.HowLong;
      this.$store.getters[
        constants.loanModel
      ].employed_position = this.employed.Position;
      this.$store.getters[
        constants.loanModel
      ].employed_present_business_address = this.employed.PresentAddress;
      this.$store.getters[
        constants.loanModel
      ].employed_tel_no = this.employed.Telephone;
      this.$store.getters[
        constants.loanModel
      ].employed_salary = this.employed.Salary;
      this.$store.getters[
        constants.loanModel
      ].employed_previous = this.employed.Previous;
      this.$store.getters[
        constants.loanModel
      ].employed_previous_business_address = this.employed.PreviousAddress;

      this.$store.getters[
        constants.loanModel
      ].business_nature = this.selfOwn.BusinessNature;
      this.$store.getters[
        constants.loanModel
      ].business_name = this.selfOwn.BusinessName;
      this.$store.getters[
        constants.loanModel
      ].business_address = this.selfOwn.BusinessAddress;
      this.$store.getters[
        constants.loanModel
      ].business_capital = this.selfOwn.Capital;
      this.$store.getters[
        constants.loanModel
      ].business_monthly_income_net = this.selfOwn.Net;
      this.$store.getters[
        constants.loanModel
      ].business_monthly_income_gross = this.selfOwn.Gross;
      this.$store.getters[
        constants.loanModel
      ].business_phone_no = this.selfOwn.PhoneNumber;
      this.$store.getters[
        constants.loanModel
      ].business_how_long = this.selfOwn.HowLong;

      this.$store.getters[
        constants.loanModel
      ].pension_agency = this.pension.Agency;
      this.$store.getters[
        constants.loanModel
      ].pension_monthly = this.pension.Monthly;

      this.$store.getters[
        constants.loanModel
      ].remittance_name = this.remittance.Name;
      this.$store.getters[
        constants.loanModel
      ].remittance_location = this.remittance.Location;
      this.$store.getters[
        constants.loanModel
      ].remittance_relationship = this.remittance.Relationship;
      this.$store.getters[
        constants.loanModel
      ].remittance_monthly_amount = this.remittance.MonthlyAmount;
      this.$store.getters[
        constants.loanModel
      ].remittance_frequency = this.remittance.Frequency;

      this.$store.getters[
        constants.loanModel
      ].father_name = this.parents.Father.Name;
      this.$store.getters[
        constants.loanModel
      ].father_age = this.parents.Father.Age;
      this.$store.getters[
        constants.loanModel
      ].father_address = this.parents.Father.Address;
      this.$store.getters[
        constants.loanModel
      ].father_facebook = this.parents.Father.Facebook;
      this.$store.getters[
        constants.loanModel
      ].father_income_source = this.parents.Father.Employment;
      this.$store.getters[
        constants.loanModel
      ].father_office_address = this.parents.Father.OfficeAddress;
      this.$store.getters[
        constants.loanModel
      ].father_position = this.parents.Father.Position;
      this.$store.getters[
        constants.loanModel
      ].father_how_long = this.parents.Father.HowLong;

      this.$store.getters[
        constants.loanModel
      ].mother_name = this.parents.Mother.Name;
      this.$store.getters[
        constants.loanModel
      ].mother_age = this.parents.Mother.Age;
      this.$store.getters[
        constants.loanModel
      ].mother_address = this.parents.Mother.Address;
      this.$store.getters[
        constants.loanModel
      ].mother_facebook = this.parents.Mother.Facebook;
      this.$store.getters[
        constants.loanModel
      ].mother_income_source = this.parents.Mother.Employment;
      this.$store.getters[
        constants.loanModel
      ].mother_office_address = this.parents.Mother.OfficeAddress;
      this.$store.getters[
        constants.loanModel
      ].mother_position = this.parents.Mother.Position;
      this.$store.getters[
        constants.loanModel
      ].mother_how_long = this.parents.Mother.HowLong;

      this.$store.getters[constants.loanModel].spouse_name = this.spouse.Name;
      this.$store.getters[constants.loanModel].spouse_age = this.spouse.Age;
      this.$store.getters[
        constants.loanModel
      ].spouse_income_source = this.spouse.IncomeSource;
      this.$store.getters[
        constants.loanModel
      ].spouse_office_address = this.spouse.OfficeAddress;
      this.$store.getters[
        constants.loanModel
      ].spouse_position = this.spouse.Position;
      this.$store.getters[
        constants.loanModel
      ].spouse_how_long = this.spouse.HowLong;
      this.$store.getters[
        constants.loanModel
      ].spouse_tel_no = this.spouse.OfficeTelNo;
      this.$store.getters[
        constants.loanModel
      ].spouse_salary = this.spouse.Salary;

      this.$store.getters[
        constants.loanModel
      ].father_in_law_name = this.parentsInLaw.Father.Name;
      this.$store.getters[
        constants.loanModel
      ].father_in_law_age = this.parentsInLaw.Father.Age;
      this.$store.getters[
        constants.loanModel
      ].father_in_law_address = this.parentsInLaw.Father.Address;
      this.$store.getters[
        constants.loanModel
      ].father_in_law_facebook = this.parentsInLaw.Father.Facebook;
      this.$store.getters[
        constants.loanModel
      ].father_in_law_income_source = this.parentsInLaw.Father.Employment;
      this.$store.getters[
        constants.loanModel
      ].father_in_law_office_address = this.parentsInLaw.Father.OfficeAddress;
      this.$store.getters[
        constants.loanModel
      ].father_in_law_position = this.parentsInLaw.Father.Position;
      this.$store.getters[
        constants.loanModel
      ].father_in_law_how_long = this.parentsInLaw.Father.HowLong;

      this.$store.getters[
        constants.loanModel
      ].mother_in_law_name = this.parentsInLaw.Mother.Name;
      this.$store.getters[
        constants.loanModel
      ].mother_in_law_age = this.parentsInLaw.Mother.Age;
      this.$store.getters[
        constants.loanModel
      ].mother_in_law_address = this.parentsInLaw.Mother.Address;
      this.$store.getters[
        constants.loanModel
      ].mother_in_law_facebook = this.parentsInLaw.Mother.Facebook;
      this.$store.getters[
        constants.loanModel
      ].mother_in_law_income_source = this.parentsInLaw.Mother.Employment;
      this.$store.getters[
        constants.loanModel
      ].mother_in_law_office_address = this.parentsInLaw.Mother.OfficeAddress;
      this.$store.getters[
        constants.loanModel
      ].mother_in_law_position = this.parentsInLaw.Mother.Position;
      this.$store.getters[
        constants.loanModel
      ].mother_in_law_how_long = this.parentsInLaw.Mother.HowLong;

      this.$store.getters[
        constants.loanModel
      ].desired_tc_terms = this.unitDesiredTC.DesiredTCTerms;
      this.$store.getters[
        constants.loanModel
      ].desired_tc_down_payment = this.unitDesiredTC.DesiredTCDownPayment;
      this.$store.getters[
        constants.loanModel
      ].desired_tc_monthly_installment = this.unitDesiredTC.DesiredTCMonthlyInstallment;
      this.$store.getters[
        constants.loanModel
      ].desired_tc_total_price = this.unitDesiredTC.DesiredTCTotalPrice;
      this.$store.getters[
        constants.loanModel
      ].desired_tc_total_rebate = this.unitDesiredTC.DesiredTCTotalRebate;
      this.$store.getters[
        constants.loanModel
      ].desired_tc_remarks = this.unitDesiredTC.DesiredTCRemarks;

      this.$store.getters[
        constants.loanModel
      ].is_agreed = this.confirmation.IsAgreed;
      this.$store.getters[
        constants.loanModel
      ].confirmation_officer = this.confirmation.ClosingOfficer;
      this.$store.getters[
        constants.loanModel
      ].confirmation_date = this.confirmation.Date;
      this.$store.getters[
        constants.loanModel
      ].confirmation_time = this.confirmation.Time;
      this.$store.getters[constants.loanModel].signature = this.signature;

      this.$store.getters[
        constants.loanModel
      ].office_use_ca_name = this.officeUse.CAName;
      this.$store.getters[
        constants.loanModel
      ].office_use_ca_remarks = this.officeUse.CARemarks;
      this.$store.getters[
        constants.loanModel
      ].office_use_ca_date = this.officeUse.CADate;
      this.$store.getters[
        constants.loanModel
      ].office_use_ca_time = this.officeUse.CATime;

      this.$store.getters[
        constants.loanModel
      ].office_use_ccs_name = this.officeUse.CCSName;
      this.$store.getters[
        constants.loanModel
      ].office_use_ccs_remarks = this.officeUse.CCSRemarks;
      this.$store.getters[
        constants.loanModel
      ].office_use_ccs_date = this.officeUse.CCSDate;
      this.$store.getters[
        constants.loanModel
      ].office_use_ccs_time = this.officeUse.CCSTime;

      this.$store.getters[
        constants.loanModel
      ].office_use_invoice_no = this.officeUse.ChargeInvoiceNumber;
      this.$store.getters[
        constants.loanModel
      ].office_use_invoice_date = this.officeUse.InvoiceDate;
      this.$store.getters[
        constants.loanModel
      ].office_use_or_no = this.officeUse.ORNumber;
      this.$store.getters[
        constants.loanModel
      ].office_use_or_date = this.officeUse.ORDate;
      this.$store.getters[
        constants.loanModel
      ].office_use_amount = this.officeUse.Amount;
      this.$store.getters[
        constants.loanModel
      ].office_use_cashier = this.officeUse.Cashier;

      this.$store.getters[
        constants.loanModel
      ].list_childrens = this.listChildren;
      this.$store.getters[
        constants.loanModel
      ].list_credit_histories = this.listCreditHistory;
      this.$store.getters[
        constants.loanModel
      ].list_personal_properties = this.listPropertyOwned;
      this.$store.getters[
        constants.loanModel
      ].list_unit_desireds = this.listUnitDesired;
      this.$store.getters[
        constants.loanModel
      ].list_unit_desired_tcs = this.listUnitDesiredTC;
      this.$store.getters[
        constants.loanModel
      ].list_attachments = this.newAttachments;
      this.$store.getters[
        constants.loanModel
      ].list_removed_attachments = this.removeAttachments;
    },
    MaritalStatusChange(e) {
      if (e == "Married") {
        this.rSpouse.Name = [v => !!v || "Spouse Name is required"];
        (this.rSpouse.Age = [
          v => !!v || "Age is required",
          v => !v || /^[0-9]+$/.test(v) || "Please input a valid age"
        ]),
          (this.rParentsInLaw.Father.Name = [
            v => !!v || "Father In Law's Name is required"
          ]);
        this.rParentsInLaw.Father.Age = [
          v => !!v || "Age is required",
          v => !v || /^[0-9]+$/.test(v) || "Please input a valid age"
        ];
        this.rParentsInLaw.Father.Address = [
          v => !!v || "Father In Law's Address is required"
        ];

        this.rParentsInLaw.Mother.Name = [
          v => !!v || "Mother In Law's Name is required"
        ];
        this.rParentsInLaw.Mother.Age = [
          v => !!v || "Age is required",
          v => !v || /^[0-9]+$/.test(v) || "Please input a valid age"
        ];
        this.rParentsInLaw.Mother.Address = [
          v => !!v || "Mother In Law's Address is required"
        ];

        this.skipSpouse = false;
        this.skipParentLaw = false;
      } else {
        this.rSpouse.Name = [];
        this.rSpouse.Age = [];

        this.rParentsInLaw.Father.Name = [];
        this.rParentsInLaw.Father.Age = [];
        this.rParentsInLaw.Father.Address = [];

        this.rParentsInLaw.Mother.Name = [];
        this.rParentsInLaw.Mother.Age = [];
        this.rParentsInLaw.Mother.Address = [];

        this.skipSpouse = true;
        this.skipParentLaw = true;
      }
    },
    ResidenceChange(e) {
      if (e == "Rented") {
        this.rPersonalInfo.RentingName = [
          v => !!v || "Landlord Name is required"
        ];
        this.rPersonalInfo.RentingAddress = [
          v => !!v || "Renting Address is required"
        ];
      } else {
        this.rPersonalInfo.RentingName = [];
        this.rPersonalInfo.RentingAddress = [];
      }
    },
    pilExpandable() {
      this.skipParentLaw = !this.skipParentLaw;
    },
    childExpandable() {
      this.skipChildren = !this.skipChildren;
    },
    spouseExpandable() {
      this.skipSpouse = !this.skipSpouse;
    },
    listUploads(e) {
      this.showAttachments = true;
      let uploads = e.srcElement.files;
      let temp = uploads.length;
      let self = this;

      if (this.countAttach + uploads.length > 5) {
        this.$refs.info.open(constants.message, constants.attachInfo, {
          color: constants.error_color
        });
        temp = 5 - this.countAttach;
        this.countAttach += temp;
      } else {
        this.countAttach += uploads.length;
      }
      for (let index = 0; index < temp; index++) {
        this.displayAttachments.push(uploads[index]);
        var reader = new FileReader();
        reader.onload = function(event) {
          var byteArray = new Uint8Array(event.target.result);
          byteArray = Array.from(byteArray);
          self.newAttachments.push({
            FileName: uploads[index].name,
            FileDataArray: byteArray
          });
        };
        // when the file is read it triggers the onload event above.
        reader.readAsArrayBuffer(uploads[index]);
      }
    },

    DeleteAttachment(index) {
      if (this.countAttach == 1) {
        console.log("Cannot Remove");
      } else {
        console.log(this.mainAttachments[index].FileName);
        this.removeAttachments.push(
          JSON.parse(JSON.stringify(this.mainAttachments[index].FileName))
        );
        this.mainAttachments.splice(index, 1);
        this.countAttach--;
      }
    },
    RemoveAttachment(index) {
      this.displayAttachments.splice(index, 1);
      this.newAttachments.splice(index, 1);
      this.countAttach--;
      if (this.countAttach == 0) {
        this.showAttachments = false;
      }
    },
    ResetFileField() {
      this.displayAttachments = [];
      this.showAttachments = false;
      this.newAttachments = [];
      this.countAttach = this.mainAttachments.length;
    },
    addChild(children) {
      if (
        !this.hasChildName &&
        !this.hasChildAge &&
        !this.hasChildHomeAddress &&
        !this.hasChildTelephone &&
        !this.hasChildEmploymentOrSchool &&
        !this.hasChildPositionOrGrade &&
        !this.hasChildAddress &&
        !this.hasChildHowLong
      ) {
        this.listChildren.push({
          ChildName: children.ChildName,
          ChildAge: children.ChildAge,
          ChildHomeAddress: children.ChildHomeAddress,
          ChildTelNo: children.ChildTelNo,
          ChildEmploySchool: children.ChildEmploySchool,
          ChildEmploySchoolAddress: children.ChildEmploySchoolAddress,
          ChildPosGrade: children.ChildPosGrade,
          ChildHowLong: children.ChildHowLong
        });
        this.toggleChildModal();
      }
    },
    editChild(index, child) {
      this.children = Object.assign({}, child);
      this.indexEditChildren = index;
      this.isEditChildren = true;
      this.isAddChildren = true;
    },
    saveChild(index, children) {
      if (
        !this.hasChildName &&
        !this.hasChildAge &&
        !this.hasChildHomeAddress &&
        !this.hasChildTelephone &&
        !this.hasChildEmploymentOrSchool &&
        !this.hasChildPositionOrGrade &&
        !this.hasChildAddress &&
        !this.hasChildHowLong
      ) {
        this.listChildren.splice(index, 1, children);
        this.toggleChildModal();
      }
    },
    delChildRow(index) {
      this.listChildren.splice(index, 1);
    },
    toggleChildModal() {
      this.children = {
        ChildName: "",
        ChildAge: "",
        ChildHomeAddress: "",
        ChildTelNo: "",
        ChildEmploySchool: "",
        ChildEmploySchoolAddress: "",
        ChildPosGrade: "",
        ChildHowLong: ""
      };
      this.isEditChildren = false;
      this.isAddChildren = !this.isAddChildren;
    },

    addCreditHistory(creditHistory) {
      if (
        !this.hasCreditHistoryName &&
        !this.hasCreditHistoryType &&
        !this.hasCreditHistoryDate &&
        !this.hasCreditHistoryTerms &&
        !this.hasCreditHistoryBalance
      ) {
        this.listCreditHistory.push({
          HistoryCompanyName: creditHistory.HistoryCompanyName,
          HistoryTypeOfUnit: creditHistory.HistoryTypeOfUnit,
          HistoryDatePurchase: creditHistory.HistoryDatePurchase,
          HistoryTerms: creditHistory.HistoryTerms,
          HistoryRemainingBalance: creditHistory.HistoryRemainingBalance
        });
        this.toggleCreditHistoryModal();
      }
    },
    editCreditHistory(index, history) {
      this.creditHistory = Object.assign({}, history);
      this.indexEditCreditHistory = index;
      this.isEditCreditHistory = true;
      this.isAddCreditHistory = true;
    },
    saveCreditHistory(index, creditHistory) {
      if (
        !this.hasCreditHistoryName &&
        !this.hasCreditHistoryType &&
        !this.hasCreditHistoryDate &&
        !this.hasCreditHistoryTerms &&
        !this.hasCreditHistoryBalance
      ) {
        this.listCreditHistory.splice(index, 1, creditHistory);
        this.toggleCreditHistoryModal();
      }
    },
    delCreditHistoryRow(index) {
      this.listCreditHistory.splice(index, 1);
    },
    toggleCreditHistoryModal() {
      this.creditHistory = {
        HistoryCompanyName: "",
        HistoryTypeOfUnit: "",
        HistoryDatePurchase: new Date().toISOString().substr(0, 10),
        HistoryTerms: "",
        HistoryRemainingBalance: ""
      };
      this.isEditCreditHistory = false;
      this.isAddCreditHistory = !this.isAddCreditHistory;
    },

    addProperty(property) {
      if (!this.hasPropertyName) {
        this.listPropertyOwned.push({
          Property: property.Property
        });
        this.togglePropertyModal();
      }
    },
    editProperty(index, properties) {
      this.property = Object.assign({}, properties);
      this.indexEditProperty = index;
      this.isEditProperty = true;
      this.isAddProperty = true;
    },
    saveProperty(index, property) {
      if (!this.hasPropertyName) {
        this.listPropertyOwned.splice(index, 1, property);
        this.togglePropertyModal();
      }
    },
    delPropertyRow(index) {
      this.listPropertyOwned.splice(index, 1);
    },
    togglePropertyModal() {
      this.property = {
        Property: ""
      };
      this.isEditProperty = false;
      this.isAddProperty = !this.isAddProperty;
    },

    addUnitDesired(unitDesired) {
      if (
        !this.hasUnitDesiredBrand &&
        !this.hasUnitDesiredSerial &&
        !this.hasUnitDesiredCode &&
        !this.hasUnitDesiredAmount &&
        !this.hasUnitDesiredAccounting
      ) {
        this.listUnitDesired.push({
          DesiredBrandModel: unitDesired.DesiredBrandModel,
          DesiredSerialNo: unitDesired.DesiredSerialNo,
          DesiredCode: unitDesired.DesiredCode,
          DesiredAmount: unitDesired.DesiredAmount,
          DesiredAccounting: unitDesired.DesiredAccounting
        });
        this.toggleUnitDesiredModal();
      }
    },
    editUnitDesired(index, desired) {
      this.unitDesired = Object.assign({}, desired);
      this.indexEditUnitDesired = index;
      this.isEditUnitDesired = true;
      this.isAddUnitDesired = true;
    },
    saveUnitDesired(index, unitDesired) {
      if (!this.hasUnitDesiredName) {
        this.listUnitDesired.splice(index, 1, unitDesired);
        this.toggleUnitDesiredModal();
      }
    },
    delUnitDesiredRow(index) {
      this.listUnitDesired.splice(index, 1);
    },
    toggleUnitDesiredModal() {
      this.unitDesired = {
        DesiredBrandModel: "",
        DesiredSerialNo: "",
        DesiredCode: "",
        DesiredAmount: "",
        DesiredAccounting: ""
      };
      this.isEditUnitDesired = false;
      this.isAddUnitDesired = !this.isAddUnitDesired;
    },

    // addUnitDesiredTC(unitDesiredTC) {
    //   if (
    //     !this.hasUnitDesiredTCBrand &&
    //     !this.hasUnitDesiredTCTerms &&
    //     !this.hasUnitDesiredTCDownPayment &&
    //     !this.hasUnitDesiredTCMonthlyInstallment &&
    //     !this.hasUnitDesiredTCTotalPrice &&
    //     !this.hasUnitDesiredTCTotalRebate &&
    //     !this.hasUnitDesiredTCRemarks
    //   ) {
    //     this.listUnitDesiredTC.push({
    //       DesiredTCBrandModel: unitDesiredTC.DesiredTCBrandModel,
    //       DesiredTCTerms: unitDesiredTC.DesiredTCTerms,
    //       DesiredTCDownPayment: unitDesiredTC.DesiredTCDownPayment,
    //       DesiredTCMonthlyInstallment:
    //         unitDesiredTC.DesiredTCMonthlyInstallment,
    //       DesiredTCTotalPrice: unitDesiredTC.DesiredTCTotalPrice,
    //       DesiredTCTotalRebate: unitDesiredTC.DesiredTCTotalRebate,
    //       DesiredTCRemarks: unitDesiredTC.DesiredTCRemarks
    //     });
    //     this.toggleUnitDesiredTCModal();
    //   }
    // },
    // editUnitDesiredTC(index, desiredTC) {
    //   this.unitDesiredTC = Object.assign({}, desiredTC);
    //   this.indexEditUnitDesiredTC = index;
    //   this.isEditUnitDesiredTC = true;
    //   this.isAddUnitDesiredTC = true;
    // },
    // saveUnitDesiredTC(index, unitDesiredTC) {
    //   if (
    //     !this.hasUnitDesiredTCBrand &&
    //     !this.hasUnitDesiredTCTerms &&
    //     !this.hasUnitDesiredTCDownPayment &&
    //     !this.hasUnitDesiredTCMonthlyInstallment &&
    //     !this.hasUnitDesiredTCTotalPrice &&
    //     !this.hasUnitDesiredTCTotalRebate &&
    //     !this.hasUnitDesiredTCRemarks
    //   ) {
    //     this.listUnitDesiredTC.splice(index, 1, unitDesiredTC);
    //     this.toggleUnitDesiredTCModal();
    //   }
    // },
    // delUnitDesiredTCRow(index) {
    //   this.listUnitDesiredTC.splice(index, 1);
    // },
    // toggleUnitDesiredTCModal() {
    //   this.unitDesiredTC = {
    //     DesiredTCBrandModel: "",
    //     DesiredTCTerms: "",
    //     DesiredTCDownPayment: "",
    //     DesiredTCMonthlyInstallment: "",
    //     DesiredTCTotalPrice: "",
    //     DesiredTCTotalRebate: "",
    //     DesiredTCRemarks: ""
    //   };
    //   this.isEditUnitDesiredTC = false;
    //   this.isAddUnitDesiredTC = !this.isAddUnitDesiredTC;
    // },
    download(loanID, attachmentFileName, attachmentType) {
      this.fullscreenLoading = true;
      var attachment = {
        loanID: loanID,
        fileName: attachmentFileName,
        type: attachmentType
      };

      this.$store
        .dispatch("application/installment/downloadAttachment", attachment)
        .then(() => {
          this.fullscreenLoading = false;
          if (this.errorMessage != null) {
            this.$refs.info.open(constants.warning, this.errorMessage, {
              color: constants.error_color
            });
          }
        });
    },

    clearStore() {
      this.$store.dispatch(constants.clearLogin);
      this.$store.dispatch(constants.clearUsers);
      this.$store.dispatch(constants.clearAccounts);
      this.$store.dispatch(constants.clearEmails);
      this.$store.dispatch(constants.clearJobOrders);
      this.$store.dispatch(constants.clearCases);
      this.$store.dispatch(constants.clearRating);
      this.$store.dispatch(constants.clearTemplate);
      this.$router.push("/login");
    },
    handleConnectivityChange(status) {
      if (status === false) {
        this.status = false;
        this.$refs.info
          .open(constants.message, constants.noInternet, {
            color: constants.error_color
          })
          .then(() => {});
      } else {
        this.status = true;
      }
    }
  },
  filters: {
    formatBytes(a, b) {
      if (0 == a) return "0 Bytes";
      var c = 1024,
        d = b || 2,
        e = ["Bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"],
        f = Math.floor(Math.log(a) / Math.log(c));
      return parseFloat((a / Math.pow(c, f)).toFixed(d)) + " " + e[f];
    }
  }
};
</script>

<style scoped>
#signature {
  border: double 3px #000;
  border-radius: 5px;
}
table {
  border-radius: 2px;
  border-collapse: collapse;
  border-spacing: 0;
  width: 100%;
  max-width: 100%;
}
@media screen and (max-width: 960px) {
  table {
    width: 930px;
  }
}
th {
  padding: 8px;
}
td {
  text-align: center;
}
td input {
  padding: 8px;
  max-width: 100%;
  width: 100%;
  text-align: center;
}

.border-none > .v-input__control > .v-input__slot:before {
  border-style: none !important;
}

input:focus,
textarea:focus,
select:focus {
  outline: none;
}
@media screen and (max-width: 960px) {
  .table-responsive {
    overflow-x: auto;
  }
}

.title-flex {
  display: flex;
  align-items: center;
}
.title-header {
  padding: 10px;
  background-color: #ff0000;
  color: #fff;
  font-size: 20px;
}
.title-bg {
  background-color: #ff0000;
}

.expandable-icon {
  border-radius: 10px;
  border: 1px solid #fff;
}

.text-center {
  text-align: center;
}
.disabled-color--bg {
  opacity: 0.8;
  /* background-color: #EBEBE4; */
}
.break-word {
  word-wrap: break-word;
}
</style>
