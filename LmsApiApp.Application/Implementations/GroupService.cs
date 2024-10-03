using AutoMapper;
using LmsApiApp.Application.Dtos.GroupDtos;
using LmsApiApp.Application.Exceptions;
using LmsApiApp.Application.Interfaces;
using LmsApiApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Implementations
{
    public class GroupService : IGroupService
    {
        private readonly IMapper _mapper;
        private readonly IGroupRepository _groupRepository;

        public GroupService(IMapper mapper, IGroupRepository groupRepository)
        {
            _mapper = mapper;
            _groupRepository = groupRepository;
        }

        public async Task<List<GroupDto>> GetAllGroupsAsync()
        {
            var groups = await _groupRepository.GetAllAsync(); 
            return _mapper.Map<List<GroupDto>>(groups); 
        }

        public async Task<GroupDto> GetGroupByIdAsync(int id)
        {
            var group = await _groupRepository.GetByIdAsync(id); 
            if (group == null) throw new GroupNotFoundException(id);
            return _mapper.Map<GroupDto>(group); 
        }

        public async Task AddGroupAsync(GroupDto groupDto)
        {
            var group = _mapper.Map<Group>(groupDto); 
            await _groupRepository.AddAsync(group); 
        }

        public async Task UpdateGroupAsync(int id, GroupDto groupDto)
        {
            var group = await _groupRepository.GetByIdAsync(id); 
            if (group == null) throw new GroupNotFoundException(id);

            _mapper.Map(groupDto, group); 
            await _groupRepository.UpdateAsync(group); 
        }

        public async Task DeleteGroupAsync(int id)
        {
            var group = await _groupRepository.GetByIdAsync(id); 
            if (group == null) throw new GroupNotFoundException(id);

            await _groupRepository.DeleteAsync(group); 
        }
    }
}
