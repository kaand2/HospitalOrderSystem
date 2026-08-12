using FluentValidation;
using FluentValidation.Results;
using HospitalOrderSystem.API.Controllers;
using HospitalOrderSystem.Application.DTOs.Patients;
using HospitalOrderSystem.Application.Interfaces.Services;
using HospitalOrderSystem.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HospitalOrderSystem.Tests.Controllers
{
    public class PatientsControllerTest
    {
        private readonly Mock<IPatientService> _mockPatientService;
        private readonly Mock<IValidator<CreatePatientDto>> _mockCreateValidator;
        private readonly Mock<IValidator<UpdatePatientDto>> _mockUpdateValidator;
        private readonly PatientsController _controller;

        public PatientsControllerTest()
        {
            _mockPatientService = new Mock<IPatientService>();
            _mockCreateValidator = new Mock<IValidator<CreatePatientDto>>();
            _mockUpdateValidator = new Mock<IValidator<UpdatePatientDto>>();

            _controller = new PatientsController(
                _mockPatientService.Object,
                _mockCreateValidator.Object,
                _mockUpdateValidator.Object);
        }

        [Fact]
        public async Task Create_WhenDtoIsValid_ReturnsCreatedAtAction()
        {
            var createDto = new CreatePatientDto 
            { 
                TcNo = "24657896248", 
                FirstName = "kaan", 
                LastName = "Doğan", 
                BirthDate = new DateTime(2003, 5, 23),
                Gender = (Gender)2,
                InsuranceType = (InsuranceType)3,
                Phone = "5355535353"
            };
            
            var returnedPatient = new PatientDto 
            { 
                Id = 1, 
                TcNo = "24657896248", 
                FirstName = "kaan", 
                LastName = "Doğan", 
                BirthDate = new DateTime(2003, 5, 23),
                Gender = (Gender)2,
                InsuranceType = (InsuranceType)3,
                Phone = "5355535353"
            };

            _mockCreateValidator
                .Setup(v => v.ValidateAsync(createDto, default))
                .ReturnsAsync(new ValidationResult());

            _mockPatientService
                .Setup(s => s.CreateAsync(createDto))
                .ReturnsAsync(returnedPatient);

            var result = await _controller.Create(createDto);

            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(201, actionResult.StatusCode);
            Assert.Equal(nameof(PatientsController.GetById), actionResult.ActionName);
            Assert.Equal(returnedPatient.Id, actionResult.RouteValues?["id"]);
            Assert.Equal(returnedPatient, actionResult.Value);

            _mockPatientService.Verify(s => s.CreateAsync(createDto), Times.Once);
        }

        [Fact]
        public async Task Create_WhenDtoIsInvalid_ReturnsBadRequestAndDoesNotCallService()
        {
            var createDto = new CreatePatientDto
            {
                TcNo = "24657896248935",
                BirthDate = new DateTime(2027, 5, 23),
                Gender = (Gender)5,
                InsuranceType = (InsuranceType)9
            };
            
            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure(nameof(CreatePatientDto.TcNo),
                 "TC No formatı geçersiz."),
                new ValidationFailure(nameof(CreatePatientDto.BirthDate),
                 "Doğum tarihi gelecekte olamaz.")
            };

            _mockCreateValidator
                .Setup(v => v.ValidateAsync(createDto, default))
                .ReturnsAsync(new ValidationResult(validationFailures));

            var result = await _controller.Create(createDto);

            var actionResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, actionResult.StatusCode);
            
            _mockPatientService.Verify(s => s.CreateAsync(It.IsAny<CreatePatientDto>()), Times.Never);
        }

        [Fact]
        public async Task Update_WhenDtoIsValid_ReturnsOkWithUpdatedPatient()
        {
            int patientId = 1;
            var updateDto = new UpdatePatientDto 
            { 
                TcNo = "24657896248", 
                FirstName = "kaan", 
                LastName = "Doğan",
                BirthDate = new DateTime(2003, 5, 23),
                Gender = (Gender)2,
                InsuranceType = (InsuranceType)3,
                Phone = "5355535353"
            };
            
            var updatedPatient = new PatientDto 
            { 
                Id = patientId, 
                TcNo = "24657896248", 
                FirstName = "kaan", 
                LastName = "Doğan",
                BirthDate = new DateTime(2003, 5, 23),
                Gender = (Gender)2,
                InsuranceType = (InsuranceType)3,
                Phone = "5355535353"
            };

            _mockUpdateValidator
                .Setup(v => v.ValidateAsync(updateDto, default))
                .ReturnsAsync(new ValidationResult());

            _mockPatientService
                .Setup(s => s.UpdateAsync(patientId, updateDto))
                .ReturnsAsync(updatedPatient);

            var result = await _controller.Update(patientId, updateDto);

            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, actionResult.StatusCode);
            Assert.Equal(updatedPatient, actionResult.Value);

            _mockPatientService.Verify(s => s.UpdateAsync(patientId, updateDto), Times.Once);
        }

        [Fact]
        public async Task Update_WhenDtoIsInvalid_ReturnsBadRequestAndDoesNotCallService()
        {
            int patientId = 1;
            var updateDto = new UpdatePatientDto
            {
                TcNo = "24657896248935",
                BirthDate = new DateTime(2027, 5, 23)
            };
            
            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure(nameof(UpdatePatientDto.TcNo),
                 "TC No formatı geçersiz.")
            };

            _mockUpdateValidator
                .Setup(v => v.ValidateAsync(updateDto, default))
                .ReturnsAsync(new ValidationResult(validationFailures));

            var result = await _controller.Update(patientId, updateDto);

            var actionResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, actionResult.StatusCode);

            _mockPatientService.Verify(s => s.UpdateAsync
            (It.IsAny<int>(), It.IsAny<UpdatePatientDto>()),
             Times.Never);
        }

        [Fact]
        public async Task GetById_WhenPatientExists_ReturnsOkWithPatient()
        {
            int patientId = 1;
            var patient = new PatientDto 
            { 
                Id = patientId, 
                TcNo = "24657896248", 
                FirstName = "kaan", 
                LastName = "Doğan",
                BirthDate = new DateTime(2003, 5, 23)
            };

            _mockPatientService
                .Setup(s => s.GetByIdAsync(patientId))
                .ReturnsAsync(patient);

            var result = await _controller.GetById(patientId);

            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, actionResult.StatusCode);
            Assert.Equal(patient, actionResult.Value);

            _mockPatientService.Verify(s => s.GetByIdAsync(patientId), Times.Once);
        }

        [Fact]
        public async Task GetById_WhenPatientDoesNotExist_ThrowsKeyNotFoundException()
        {
            int patientId = 50;

            _mockPatientService
                .Setup(s => s.GetByIdAsync(patientId))
                .ThrowsAsync(new KeyNotFoundException("Hasta bulunamadı."));

            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _controller.GetById(patientId));
            
            _mockPatientService.Verify(s => s.GetByIdAsync(patientId), Times.Once);
        }

        [Fact]
        public async Task Update_WhenPatientDoesNotExist_ThrowsKeyNotFoundException()
        {
            int patientId = 50;
            var updateDto = new UpdatePatientDto 
            { 
                TcNo = "24657896248", 
                FirstName = "kaan", 
                LastName = "Doğan",
                BirthDate = new DateTime(2003, 5, 23)
            };

            _mockUpdateValidator
                .Setup(v => v.ValidateAsync(updateDto, default))
                .ReturnsAsync(new ValidationResult());

            _mockPatientService
                .Setup(s => s.UpdateAsync(patientId, updateDto))
                .ThrowsAsync(new KeyNotFoundException("Hasta bulunamadı."));

            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _controller.Update(patientId, updateDto));

            _mockPatientService.Verify(s => s.UpdateAsync(patientId, updateDto), Times.Once);
        }
    }
}
