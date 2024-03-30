import React, { Component } from 'react';
import authService from '../../components/api-authorization/AuthorizeService';

export class EmployeeEdit extends Component {
  static displayName = EmployeeEdit.name;

  constructor(props) {
    super(props);
      this.state = { id: 0, fullName: '', birthdate: '', tin: '', employeeTypeId: 1, loading: true, loadingSave: false, errorData: null };
  }

  componentDidMount() {
    this.getEmployee(this.props.match.params.id);
  }
  handleChange(event) {
    this.setState({ [event.target.name] : event.target.value});
  }

  handleSubmit(e){
      e.preventDefault();
      if (window.confirm("Are you sure you want to save?")) {
        this.saveEmployee();
      } 
  }

  render() {

    let contents = this.state.loading
    ? <p><em>Loading...</em></p>
    : <div>
    <form>
<div className='form-row'>
<div className='form-group col-md-6'>
  <label htmlFor='inputFullName4'>Full Name: *</label>
  <input type='text' className='form-control' id='inputFullName4' onChange={this.handleChange.bind(this)} name="fullName" value={this.state.fullName} placeholder='Full Name' />
                        {this.state.errorData && this.state.errorData.errors && this.state.errorData.errors.FullName && (
                            <span style={{ color: 'red' }}>{this.state.errorData.errors.FullName[0]}</span>
                        )}
</div>
<div className='form-group col-md-6'>
  <label htmlFor='inputBirthdate4'>Birthdate: *</label>
  <input type='date' className='form-control' id='inputBirthdate4' onChange={this.handleChange.bind(this)} name="birthdate" value={this.state.birthdate} placeholder='Birthdate' />
                        {this.state.errorData && this.state.errorData.errors && this.state.errorData.errors.Birthdate && (
                            <span style={{ color: 'red' }}>{this.state.errorData.errors.Birthdate[0]}</span>
                        )}
</div>
</div>
<div className="form-row">
<div className='form-group col-md-6'>
  <label htmlFor='inputTin4'>TIN: *</label>
  <input type='text' className='form-control' id='inputTin4' onChange={this.handleChange.bind(this)} value={this.state.tin} name="tin" placeholder='TIN' />
                        {this.state.errorData && this.state.errorData.errors && this.state.errorData.errors.Tin && (
                            <span style={{ color: 'red' }}>{this.state.errorData.errors.Tin[0]}</span>
                        )}
</div>
<div className='form-group col-md-6'>
  <label htmlFor='inputEmployeeType4'>Employee Type: *</label>
  <select id='inputEmployeeType4' onChange={this.handleChange.bind(this)} value={this.state.employeeTypeId}  name="employeeTypeId" className='form-control'>
    <option value='1'>Regular</option>
    <option value='2'>Contractual</option>
    <option value='3'>Probationary</option>
    <option value='4'>PartTime</option>
  </select>
</div>
</div>
<button type="submit" onClick={this.handleSubmit.bind(this)} disabled={this.state.loadingSave} className="btn btn-primary mr-2">{this.state.loadingSave?"Loading...": "Save"}</button>
<button type="button" onClick={() => this.props.history.push("/employees/index")} className="btn btn-primary">Back</button>
</form>
</div>;


    return (
        <div>
        <h1 id="tabelLabel" >Employee Edit</h1>
        <p>All fields are required</p>
        {contents}
      </div>
    );
  }

  async saveEmployee() {
    this.setState({ loadingSave: true });
    const token = await authService.getAccessToken();
    const requestOptions = {
        method: 'PUT',
        headers: !token ? {} : { 'Authorization': `Bearer ${token}`,'Content-Type': 'application/json' },
        body: JSON.stringify(this.state)
    };
    const response = await fetch('api/employees/' + this.state.id,requestOptions);

    try {
        if (response.status === 200) {
            this.setState({ loadingSave: false });
            alert("Employee successfully saved");
            this.props.history.push("/employees/index");
        } else {
            this.setState({ loadingSave: false });
            this.setState({ errorData: await response.json() });
            console.log(this.state.errorData.errors);
        }
    } catch (error) {
        this.setState({ loadingSave: false });
        alert(error.message || 'An error occurred while processing the request.');
    }
  }

  async getEmployee(id) {
    this.setState({ loading: true,loadingSave: false });
    const token = await authService.getAccessToken();
    const response = await fetch('api/employees/' + id, {
      headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });
    const data = await response.json();
      this.setState({ id: data.id, fullName: data.fullName, birthdate: data.birthdate, tin: data.tin, employeeTypeId: data.employeeTypeId, loading: false,loadingSave: false });
  }
}
